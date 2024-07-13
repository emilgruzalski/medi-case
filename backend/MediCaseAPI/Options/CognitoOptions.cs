namespace MediCaseAPI.Options;

public class CognitoOptions
{
    public const string SectionName = "Cognito";

    public string Region { get; set; } = null!;
    public string UserPoolId { get; set; } = null!;
    public string ClientId { get; set; } = null!;

    /// <summary>Overrides the token issuer, e.g. for a local Cognito emulator.</summary>
    public string? Issuer { get; set; }

    /// <summary>Endpoint of a local Cognito emulator used instead of the AWS API.</summary>
    public string? ServiceUrl { get; set; }

    public string GetIssuer() => Issuer ?? $"https://cognito-idp.{Region}.amazonaws.com/{UserPoolId}";

    public string GetJwksUrl() => $"{GetIssuer()}/.well-known/jwks.json";
}
