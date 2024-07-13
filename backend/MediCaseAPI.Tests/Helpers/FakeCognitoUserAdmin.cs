using MediCaseAPI.Services;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Tests.Helpers;

public class FakeCognitoUserAdmin : ICognitoUserAdmin
{
    private readonly Dictionary<string, CognitoUserProfile> _users = new();

    public void Add(CognitoUserProfile profile) => _users[profile.Username] = profile;

    public Task<CognitoUserProfile> GetUserAsync(string username) => Task.FromResult(_users[username]);
}
