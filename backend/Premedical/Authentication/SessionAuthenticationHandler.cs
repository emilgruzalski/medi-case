using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Premedical.Services.Interfaces;

namespace Premedical.Authentication;

public class SessionAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ISessionService _sessionService;

    public SessionAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ISessionService sessionService)
        : base(options, logger, encoder, clock)
    {
        _sessionService = sessionService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? header = Request.Headers.Authorization;
        if (string.IsNullOrEmpty(header) ||
            !header.StartsWith(SessionAuthenticationDefaults.TokenPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }

        var token = header[SessionAuthenticationDefaults.TokenPrefix.Length..].Trim();
        var session = await _sessionService.GetActiveSessionAsync(token);
        if (session is null)
            return AuthenticateResult.Fail("Invalid or expired session.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, session.UserId.ToString()),
            new Claim(ClaimTypes.Email, session.User.Email),
            new Claim(ClaimTypes.Role, session.User.Role),
            new Claim(SessionAuthenticationDefaults.SessionIdClaim, session.Id.ToString())
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));

        return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
    }
}
