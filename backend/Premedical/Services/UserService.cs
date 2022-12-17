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

    private async Task<User> GetUserOrThrowAsync(int id) =>
        await _userRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"User with id {id} was not found.");
}
