namespace MediCaseAPI.Authentication;

public static class PasswordRules
{
    public const int MinLength = 8;
    public const int MaxLength = 128;
    public const int BCryptWorkFactor = 11;
}
