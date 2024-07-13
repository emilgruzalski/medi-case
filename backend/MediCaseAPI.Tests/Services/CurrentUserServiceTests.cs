using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Authentication;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories;
using MediCaseAPI.Services;
using MediCaseAPI.Tests.Helpers;

namespace MediCaseAPI.Tests.Services;

public class CurrentUserServiceTests
{
    private readonly MedicalContext _context = TestDatabase.Create(TestDatabase.UniqueName());
    private readonly FakeCognitoUserAdmin _cognito = new();

    private CurrentUserService CreateService() => new(new UserRepository(_context), _cognito);

    [Fact]
    public async Task GetOrCreateAsync_CreatesUserFromCognitoProfile()
    {
        _cognito.Add(new CognitoUserProfile("user-1", "Anna@Example.com", "Anna", "Nowak"));

        var user = await CreateService().GetOrCreateAsync(TestPrincipal.Create("user-1", "Moderator"));

        Assert.Equal("anna@example.com", user.Email);
        Assert.Equal(Roles.Moderator, user.Role);
        Assert.Equal("user-1", (await _context.Users.SingleAsync()).CognitoUsername);
    }

    [Fact]
    public async Task GetOrCreateAsync_LinksAccountCreatedBeforeCognito()
    {
        _context.Users.Add(new User
        {
            Email = "jan@example.com", FirstName = "Jan", LastName = "Kowalski", Role = Roles.User, CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        _cognito.Add(new CognitoUserProfile("user-2", "jan@example.com", "Jan", "Kowalski"));

        await CreateService().GetOrCreateAsync(TestPrincipal.Create("user-2"));

        var user = await _context.Users.SingleAsync();
        Assert.Equal("user-2", user.CognitoUsername);
    }

    [Fact]
    public async Task GetOrCreateAsync_UpdatesRoleFromGroups()
    {
        _cognito.Add(new CognitoUserProfile("user-3", "ola@example.com", "Ola", "Lis"));
        var service = CreateService();
        await service.GetOrCreateAsync(TestPrincipal.Create("user-3", "Admin"));

        var user = await service.GetOrCreateAsync(TestPrincipal.Create("user-3"));

        Assert.Equal(Roles.User, user.Role);
    }
}
