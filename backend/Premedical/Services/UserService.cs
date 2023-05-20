using AutoMapper;
using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Models;
using Premedical.Repositories.Interfaces;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IGroupRepository groupRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<UserDTO> RegisterAsync(UserRegisterDTO dto)
    {
        var user = _mapper.Map<User>(dto);

        if (await _userRepository.EmailExistsAsync(user.Email))
            throw new ConflictException("User with this email already exists.");

        user.PasswordHash = _passwordHasher.Hash(dto.Password);
        user.Role = Roles.User;
        user.CreatedAt = DateTime.UtcNow;

        _userRepository.Add(user);
        await _userRepository.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> GetByIdAsync(int id)
    {
        var user = await GetUserOrThrowAsync(id);
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync() =>
        _mapper.Map<IEnumerable<UserDTO>>(await _userRepository.GetAllAsync());

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

    public async Task DeleteAsync(int id, int currentUserId)
    {
        if (id == currentUserId)
            throw new BadRequestException("You cannot delete your own account.");

        var user = await GetUserOrThrowAsync(id);
        _userRepository.Remove(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<UserDTO> UpdateProfileAsync(int id, UserUpdateDTO dto)
    {
        var user = await GetUserOrThrowAsync(id);
        var email = dto.Email.Trim().ToLowerInvariant();

        if (email != user.Email && await _userRepository.EmailExistsAsync(email))
            throw new ConflictException("User with this email already exists.");

        user.Email = email;
        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        await _userRepository.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }

    public async Task ChangePasswordAsync(int id, UserPasswordDTO dto)
    {
        var user = await GetUserOrThrowAsync(id);

        if (!_passwordHasher.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new BadRequestException("Current password is incorrect.");

        user.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
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
