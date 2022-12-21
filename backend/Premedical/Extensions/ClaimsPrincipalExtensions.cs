using System.Security.Claims;
using Premedical.Authentication;

namespace Premedical.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal principal) =>
        int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public static int GetSessionId(this ClaimsPrincipal principal) =>
        int.Parse(principal.FindFirstValue(SessionAuthenticationDefaults.SessionIdClaim)!);
}
