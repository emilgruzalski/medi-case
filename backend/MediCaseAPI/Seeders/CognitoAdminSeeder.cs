using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using MediCaseAPI.Authentication;
using MediCaseAPI.Models;
using MediCaseAPI.Options;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Seeders;

/// <summary>
/// Creates the role groups and an administrator in Cognito, so a new environment can be used right away.
/// </summary>
public class CognitoAdminSeeder
{
    private const string AdminFirstName = "Admin";
    private const string AdminLastName = "MediCase";
    private static readonly string[] RoleGroups = { Roles.Admin, Roles.Moderator };

    private readonly IAmazonCognitoIdentityProvider _cognito;
    private readonly ICognitoUserAdmin _cognitoUserAdmin;
    private readonly MedicalContext _context;
    private readonly CognitoOptions _options;
    private readonly ILogger<CognitoAdminSeeder> _logger;

    public CognitoAdminSeeder(
        IAmazonCognitoIdentityProvider cognito,
        ICognitoUserAdmin cognitoUserAdmin,
        MedicalContext context,
        IOptions<CognitoOptions> options,
        ILogger<CognitoAdminSeeder> logger)
    {
        _cognito = cognito;
        _cognitoUserAdmin = cognitoUserAdmin;
        _context = context;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SeedAsync(string email, string password)
    {
        foreach (var group in RoleGroups)
            await EnsureGroupAsync(group);

        var username = await FindUsernameAsync(email) ?? await CreateUserAsync(email, password);
        await _cognitoUserAdmin.SetRoleAsync(username, Roles.Admin);
        await EnsureLocalUserAsync(email, username);
    }

    private async Task EnsureGroupAsync(string group)
    {
        try
        {
            await _cognito.CreateGroupAsync(new CreateGroupRequest { UserPoolId = _options.UserPoolId, GroupName = group });
        }
        catch (GroupExistsException)
        {
            // The group was created earlier, e.g. by Terraform.
        }
    }

    private async Task<string?> FindUsernameAsync(string email)
    {
        try
        {
            return (await _cognitoUserAdmin.GetUserAsync(email)).Username;
        }
        catch (UserNotFoundException)
        {
            return null;
        }
    }

    private async Task<string> CreateUserAsync(string email, string password)
    {
        var response = await _cognito.AdminCreateUserAsync(new AdminCreateUserRequest
        {
            UserPoolId = _options.UserPoolId,
            Username = email,
            MessageAction = MessageActionType.SUPPRESS,
            UserAttributes = new List<AttributeType>
            {
                new() { Name = CognitoAttributes.Email, Value = email },
                new() { Name = CognitoAttributes.EmailVerified, Value = "true" },
                new() { Name = CognitoAttributes.GivenName, Value = AdminFirstName },
                new() { Name = CognitoAttributes.FamilyName, Value = AdminLastName }
            }
        });
        await _cognito.AdminSetUserPasswordAsync(new AdminSetUserPasswordRequest
        {
            UserPoolId = _options.UserPoolId,
            Username = response.User.Username,
            Password = password,
            Permanent = true
        });

        _logger.LogInformation("Created administrator {Email} in Cognito", email);
        return response.User.Username;
    }

    private async Task EnsureLocalUserAsync(string email, string username)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user is null)
        {
            user = new User
            {
                Email = email,
                FirstName = AdminFirstName,
                LastName = AdminLastName,
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
        }

        user.CognitoUsername = username;
        user.Role = Roles.Admin;
        await _context.SaveChangesAsync();
    }
}
