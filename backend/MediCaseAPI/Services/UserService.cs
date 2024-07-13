using System.Security.Claims;
using AutoMapper;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IGroupRepository groupRepository,
        IPasswordHasher passwordHasher,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _passwordHasher = passwordHasher;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<UserDTO> GetCurrentAsync(ClaimsPrincipal principal) =>
        _mapper.Map<UserDTO>(await _currentUserService.GetOrCreateAsync(principal));

    public async Task<UserDTO> UpdateCurrentAsync(ClaimsPrincipal principal, UserUpdateDTO dto)
    {
        var user = await _currentUserService.GetOrCreateAsync(principal);
        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        await _userRepository.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> GetByIdAsync(int id)
    {
        var user = await GetUserOrThrowAsync(id);
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<PagedResultDTO<UserDTO>> GetPageAsync(PagingParameters parameters)
    {
        var search = parameters.NormalizedSearch;
        var users = await _userRepository.GetPageAsync(search, parameters.Skip, parameters.PageSize);
        var total = await _userRepository.CountAsync(search);
        return new PagedResultDTO<UserDTO>(_mapper.Map<IEnumerable<UserDTO>>(users), total, parameters);
    }

    public async Task<UserDTO> CreateAsync(UserAdminPostDTO dto)
    {
        var email = NormalizeEmail(dto.Email);
        if (await _userRepository.EmailExistsAsync(email))
            throw new ConflictException("User with this email already exists.");

        var user = new User
        {
            PasswordHash = _passwordHasher.Hash(dto.Password),
            CreatedAt = DateTime.UtcNow
        };
        await ApplyAdminChangesAsync(user, dto, email);

        _userRepository.Add(user);
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> UpdateAsync(int id, UserAdminUpdateDTO dto)
    {
        var user = await GetUserOrThrowAsync(id);
        var email = NormalizeEmail(dto.Email);
        if (email != user.Email && await _userRepository.EmailExistsAsync(email))
            throw new ConflictException("User with this email already exists.");

        await ApplyAdminChangesAsync(user, dto, email);
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserDTO>(user);
    }

    public async Task DeleteAsync(int id, ClaimsPrincipal principal)
    {
        var currentUser = await _currentUserService.GetOrCreateAsync(principal);
        if (id == currentUser.Id)
            throw new BadRequestException("You cannot delete your own account.");

        var user = await GetUserOrThrowAsync(id);
        _userRepository.Remove(user);
        await _userRepository.SaveChangesAsync();
    }

    private async Task ApplyAdminChangesAsync(User user, UserAdminUpdateDTO dto, string email)
    {
        if (!Roles.All.Contains(dto.Role))
            throw new BadRequestException($"Role '{dto.Role}' does not exist.");

        var groupIds = dto.GroupIds.Distinct().ToList();
        var groups = await _groupRepository.GetByIdsAsync(groupIds);
        if (groups.Count != groupIds.Count)
            throw new BadRequestException("One or more groups do not exist.");

        user.Email = email;
        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        user.Role = dto.Role;
        user.Groups.Clear();
        foreach (var group in groups)
            user.Groups.Add(group);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private async Task<User> GetUserOrThrowAsync(int id) =>
        await _userRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"User with id {id} was not found.");
}
