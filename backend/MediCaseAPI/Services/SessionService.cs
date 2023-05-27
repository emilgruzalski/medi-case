using System.Security.Cryptography;
using AutoMapper;
using Microsoft.Extensions.Options;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Models;
using MediCaseAPI.Options;
using MediCaseAPI.Repositories.Interfaces;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class SessionService : ISessionService
{
    private const string InvalidCredentialsMessage = "Invalid email or password.";

    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly SessionTokenOptions _options;

    public SessionService(
        ISessionRepository sessionRepository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper,
        IOptions<SessionTokenOptions> options)
    {
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _options = options.Value;
    }

    public async Task<SessionDTO> LoginAsync(LoginDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email.Trim().ToLowerInvariant());
        if (user is null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedException(InvalidCredentialsMessage);

        var now = DateTime.UtcNow;
        var session = new Session
        {
            UserId = user.Id,
            Token = GenerateToken(),
            CreatedAt = now,
            ExpiresAt = now.AddHours(_options.LifetimeHours)
        };
        user.LastLoginAt = now;

        _sessionRepository.RemoveRange(await _sessionRepository.GetExpiredAsync(user.Id, now));
        _sessionRepository.Add(session);
        await _sessionRepository.SaveChangesAsync();

        return new SessionDTO
        {
            Token = session.Token,
            ExpiresAt = session.ExpiresAt,
            User = _mapper.Map<UserDTO>(user)
        };
    }

    public async Task<Session?> GetActiveSessionAsync(string token)
    {
        var session = await _sessionRepository.GetByTokenAsync(token);
        return session is not null && session.ExpiresAt > DateTime.UtcNow ? session : null;
    }

    public async Task LogoutAsync(int sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session is null)
            return;

        _sessionRepository.Remove(session);
        await _sessionRepository.SaveChangesAsync();
    }

    private string GenerateToken() =>
        Convert.ToHexString(RandomNumberGenerator.GetBytes(_options.TokenBytes));
}
