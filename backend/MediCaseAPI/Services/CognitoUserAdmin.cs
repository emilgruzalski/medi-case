using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using MediCaseAPI.Authentication;
using MediCaseAPI.Options;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class CognitoUserAdmin : ICognitoUserAdmin
{
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private readonly CognitoOptions _options;

    public CognitoUserAdmin(IAmazonCognitoIdentityProvider cognito, IOptions<CognitoOptions> options)
    {
        _cognito = cognito;
        _options = options.Value;
    }

    public async Task<CognitoUserProfile> GetUserAsync(string username)
    {
        var response = await _cognito.AdminGetUserAsync(new AdminGetUserRequest
        {
            UserPoolId = _options.UserPoolId,
            Username = username
        });

        return ToProfile(response.Username, response.UserAttributes);
    }

    private static CognitoUserProfile ToProfile(string username, IReadOnlyCollection<AttributeType> attributes)
    {
        string Value(string name) => attributes.FirstOrDefault(a => a.Name == name)?.Value ?? string.Empty;
        return new CognitoUserProfile(
            username, Value(CognitoAttributes.Email), Value(CognitoAttributes.GivenName), Value(CognitoAttributes.FamilyName));
    }
}
