using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using MediCaseAPI.Authentication;
using MediCaseAPI.Options;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class CognitoUserAdmin : ICognitoUserAdmin
{
    private const string TrueValue = "true";

    // Cognito groups that represent application roles. Learners have no group.
    private static readonly string[] RoleGroups = { Roles.Admin, Roles.Moderator };

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

    public async Task<CognitoUserProfile> InviteUserAsync(string email, string firstName, string lastName)
    {
        var response = await _cognito.AdminCreateUserAsync(new AdminCreateUserRequest
        {
            UserPoolId = _options.UserPoolId,
            Username = email,
            UserAttributes = Attributes(email, firstName, lastName),
            DesiredDeliveryMediums = new List<string> { DeliveryMediumType.EMAIL }
        });

        return ToProfile(response.User.Username, response.User.Attributes);
    }

    public Task UpdateUserAsync(string username, string email, string firstName, string lastName) =>
        _cognito.AdminUpdateUserAttributesAsync(new AdminUpdateUserAttributesRequest
        {
            UserPoolId = _options.UserPoolId,
            Username = username,
            UserAttributes = Attributes(email, firstName, lastName)
        });

    public async Task SetRoleAsync(string username, string role)
    {
        var current = await _cognito.AdminListGroupsForUserAsync(new AdminListGroupsForUserRequest
        {
            UserPoolId = _options.UserPoolId,
            Username = username
        });

        foreach (var group in current.Groups.Select(g => g.GroupName).Where(g => RoleGroups.Contains(g) && g != role))
        {
            await _cognito.AdminRemoveUserFromGroupAsync(new AdminRemoveUserFromGroupRequest
            {
                UserPoolId = _options.UserPoolId,
                Username = username,
                GroupName = group
            });
        }

        if (RoleGroups.Contains(role) && current.Groups.All(g => g.GroupName != role))
        {
            await _cognito.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
            {
                UserPoolId = _options.UserPoolId,
                Username = username,
                GroupName = role
            });
        }
    }

    public async Task DeleteUserAsync(string username)
    {
        try
        {
            await _cognito.AdminDeleteUserAsync(new AdminDeleteUserRequest
            {
                UserPoolId = _options.UserPoolId,
                Username = username
            });
        }
        catch (UserNotFoundException)
        {
            // The account was already removed in Cognito.
        }
    }

    private static List<AttributeType> Attributes(string email, string firstName, string lastName) => new()
    {
        new AttributeType { Name = CognitoAttributes.Email, Value = email },
        new AttributeType { Name = CognitoAttributes.EmailVerified, Value = TrueValue },
        new AttributeType { Name = CognitoAttributes.GivenName, Value = firstName },
        new AttributeType { Name = CognitoAttributes.FamilyName, Value = lastName }
    };

    private static CognitoUserProfile ToProfile(string username, IReadOnlyCollection<AttributeType> attributes)
    {
        string Value(string name) => attributes.FirstOrDefault(a => a.Name == name)?.Value ?? string.Empty;
        return new CognitoUserProfile(
            username, Value(CognitoAttributes.Email), Value(CognitoAttributes.GivenName), Value(CognitoAttributes.FamilyName));
    }
}
