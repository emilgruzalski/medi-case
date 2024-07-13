using System.Security.Claims;
using MediCaseAPI.Authentication;

namespace MediCaseAPI.Tests.Helpers;

public static class TestPrincipal
{
    public static ClaimsPrincipal Create(string username, params string[] groups)
    {
        var claims = new List<Claim> { new(CognitoClaims.Username, username) };
        claims.AddRange(groups.Select(group => new Claim(CognitoClaims.Groups, group)));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
    }
}
