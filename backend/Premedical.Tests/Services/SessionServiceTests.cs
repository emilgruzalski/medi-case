using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Models;
using Premedical.Options;
using Premedical.Repositories;
using Premedical.Services;
using Premedical.Tests.Helpers;

namespace Premedical.Tests.Services;

public class SessionServiceTests
{
    private const string Password = "correct-password";

    private readonly MedicalContext _context = TestDatabase.Create(TestDatabase.UniqueName());
    private readonly PasswordHasher _passwordHasher = new();

    private SessionService CreateService() => new(
        new SessionRepository(_context),
        new UserRepository(_context),
        _passwordHasher,
        TestMapper.Create(),
        Microsoft.Extensions.Options.Options.Create(new SessionTokenOptions { LifetimeHours = 1, TokenBytes = 32 }));

    private async Task<User> AddUserAsync()
    {
        var user = new User
        {
            Email = "jan@example.com",
            PasswordHash = _passwordHasher.Hash(Password),
            FirstName = "Jan",
            LastName = "Kowalski",
            Role = Roles.User,
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_Throws()
    {
        await AddUserAsync();

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            CreateService().LoginAsync(new LoginDTO { Email = "jan@example.com", Password = "wrong-password" }));
    }

    [Fact]
    public async Task LoginAsync_ReturnsActiveSession()
    {
        await AddUserAsync();
        var service = CreateService();

        var session = await service.LoginAsync(new LoginDTO { Email = "JAN@example.com", Password = Password });

        Assert.NotNull(await service.GetActiveSessionAsync(session.Token));
    }

    [Fact]
    public async Task GetActiveSessionAsync_ReturnsNullForExpiredSession()
    {
        var user = await AddUserAsync();
        _context.Sessions.Add(new Session
        {
            UserId = user.Id,
            Token = "expired",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            ExpiresAt = DateTime.UtcNow.AddDays(-1)
        });
        await _context.SaveChangesAsync();

        Assert.Null(await CreateService().GetActiveSessionAsync("expired"));
    }
}
