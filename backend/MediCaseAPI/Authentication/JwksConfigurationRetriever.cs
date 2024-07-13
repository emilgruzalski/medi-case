using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace MediCaseAPI.Authentication;

/// <summary>
/// Builds the token validation configuration straight from the JWKS document of the user pool.
/// The discovery document is not needed, which also lets the API work with a local Cognito emulator.
/// </summary>
public class JwksConfigurationRetriever : IConfigurationRetriever<OpenIdConnectConfiguration>
{
    private readonly string _issuer;

    public JwksConfigurationRetriever(string issuer)
    {
        _issuer = issuer;
    }

    public async Task<OpenIdConnectConfiguration> GetConfigurationAsync(
        string address, IDocumentRetriever retriever, CancellationToken cancel)
    {
        var document = await retriever.GetDocumentAsync(address, cancel);
        var configuration = new OpenIdConnectConfiguration
        {
            Issuer = _issuer,
            JwksUri = address,
            JsonWebKeySet = new JsonWebKeySet(document)
        };

        foreach (var key in configuration.JsonWebKeySet.GetSigningKeys())
            configuration.SigningKeys.Add(key);

        return configuration;
    }
}
