namespace MediCaseAPI.Services.Interfaces;

public interface ICognitoUserAdmin
{
    Task<CognitoUserProfile> GetUserAsync(string username);
}
