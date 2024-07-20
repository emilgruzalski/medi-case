namespace MediCaseAPI.Services.Interfaces;

public interface ICognitoUserAdmin
{
    Task<CognitoUserProfile> GetUserAsync(string username);

    /// <summary>Creates the account and sends an invitation with a temporary password.</summary>
    Task<CognitoUserProfile> InviteUserAsync(string email, string firstName, string lastName);

    Task UpdateUserAsync(string username, string email, string firstName, string lastName);
    Task SetRoleAsync(string username, string role);
    Task DeleteUserAsync(string username);
}
