using System.Security.Claims;
using MediCaseAPI.Authentication;

namespace MediCaseAPI.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetCognitoUsername(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(CognitoClaims.Username)
        ?? throw new InvalidOperationException("The token has no username.");

    public static IEnumerable<string> GetCognitoGroups(this ClaimsPrincipal principal) =>
        principal.FindAll(CognitoClaims.Groups).Select(c => c.Value);

    public static DateTime? GetAuthTime(this ClaimsPrincipal principal) =>
        long.TryParse(principal.FindFirstValue(CognitoClaims.AuthTime), out var seconds)
            ? DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime
            : null;
}
