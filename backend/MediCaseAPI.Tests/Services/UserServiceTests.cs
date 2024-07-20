using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories;
using MediCaseAPI.Services;
using MediCaseAPI.Tests.Helpers;

namespace MediCaseAPI.Tests.Services;

public class UserServiceTests
{
    private readonly MedicalContext _context = TestDatabase.Create(TestDatabase.UniqueName());
    private readonly FakeCognitoUserAdmin _cognito = new();

    private UserService CreateService()
    {
        var users = new UserRepository(_context);
        return new UserService(users, new GroupRepository(_context), new CurrentUserService(users, _cognito), _cognito,
            TestMapper.Create());
    }

    [Fact]
    public async Task CreateAsync_InvitesUserInCognitoAndSetsRole()
    {
        var user = await CreateService().CreateAsync(new UserAdminPostDTO
        {
            Email = "Ewa@Example.com", FirstName = "Ewa", LastName = "Wolska", Role = Roles.Moderator
        });

        var saved = await _context.Users.SingleAsync();
        Assert.Equal("ewa@example.com", user.Email);
        Assert.NotNull(saved.CognitoUsername);
        Assert.Equal(Roles.Moderator, _cognito.Roles[saved.CognitoUsername!]);
    }

    [Fact]
    public async Task DeleteAsync_DoesNotAllowDeletingYourself()
    {
        _cognito.Add(new CognitoUserProfile("admin-1", "admin@example.com", "Ada", "Admin"));
        var service = CreateService();
        var me = await service.GetCurrentAsync(TestPrincipal.Create("admin-1", Roles.Admin));

        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.DeleteAsync(me.Id, TestPrincipal.Create("admin-1", Roles.Admin)));
    }
}
