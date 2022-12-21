namespace Premedical.Authentication;

public static class SessionAuthenticationDefaults
{
    public const string Scheme = "Session";
    public const string TokenPrefix = "Bearer ";
    public const string SessionIdClaim = "session_id";
}
