using MediCaseAPI.Services;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Tests.Helpers;

public class FakeCognitoUserAdmin : ICognitoUserAdmin
{
    private readonly Dictionary<string, CognitoUserProfile> _users = new();

    public Dictionary<string, string> Roles { get; } = new();

    public void Add(CognitoUserProfile profile) => _users[profile.Username] = profile;

    public Task<CognitoUserProfile> GetUserAsync(string username) => Task.FromResult(_users[username]);

    public Task<CognitoUserProfile> InviteUserAsync(string email, string firstName, string lastName)
    {
        var profile = new CognitoUserProfile(Guid.NewGuid().ToString(), email, firstName, lastName);
        Add(profile);
        return Task.FromResult(profile);
    }

    public Task UpdateUserAsync(string username, string email, string firstName, string lastName)
    {
        Add(new CognitoUserProfile(username, email, firstName, lastName));
        return Task.CompletedTask;
    }

    public Task SetRoleAsync(string username, string role)
    {
        Roles[username] = role;
        return Task.CompletedTask;
    }

    public Task DeleteUserAsync(string username)
    {
        _users.Remove(username);
        return Task.CompletedTask;
    }
}
