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
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _userRepository = userRepository;
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

    private async Task<User> GetUserOrThrowAsync(int id) =>
        await _userRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"User with id {id} was not found.");
}
