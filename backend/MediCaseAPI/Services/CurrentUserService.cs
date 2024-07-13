using System.Security.Claims;
using MediCaseAPI.Authentication;
using MediCaseAPI.Extensions;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICognitoUserAdmin _cognitoUserAdmin;

    public CurrentUserService(IUserRepository userRepository, ICognitoUserAdmin cognitoUserAdmin)
    {
        _userRepository = userRepository;
        _cognitoUserAdmin = cognitoUserAdmin;
    }

    public async Task<User> GetOrCreateAsync(ClaimsPrincipal principal)
    {
        var username = principal.GetCognitoUsername();
        var user = await _userRepository.GetByCognitoUsernameAsync(username) ?? await LinkOrCreateAsync(username);

        user.Role = RoleResolver.FromGroups(principal.GetCognitoGroups());
        var authTime = principal.GetAuthTime();
        if (authTime > user.LastLoginAt || (authTime.HasValue && user.LastLoginAt is null))
            user.LastLoginAt = authTime;

        await _userRepository.SaveChangesAsync();
        return user;
    }

    private async Task<User> LinkOrCreateAsync(string username)
    {
        var profile = await _cognitoUserAdmin.GetUserAsync(username);
        var email = profile.Email.Trim().ToLowerInvariant();

        // Accounts created before the move to Cognito are linked by their e-mail address.
        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null)
        {
            user = new User
            {
                Email = email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Role = Roles.User,
                CreatedAt = DateTime.UtcNow
            };
            _userRepository.Add(user);
        }

        user.CognitoUsername = username;
        return user;
    }
}
