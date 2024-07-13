using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using MediCaseAPI.Authentication;
using MediCaseAPI.Options;

namespace MediCaseAPI.Extensions;

public static class CognitoAuthenticationExtensions
{
    public static IServiceCollection AddCognitoAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(CognitoOptions.SectionName);
        var cognito = section.Get<CognitoOptions>()
            ?? throw new InvalidOperationException("The Cognito configuration section is missing.");
        services.Configure<CognitoOptions>(section);

        var issuer = cognito.GetIssuer();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    cognito.GetJwksUrl(),
                    new JwksConfigurationRetriever(issuer),
                    new HttpDocumentRetriever { RequireHttps = issuer.StartsWith(Uri.UriSchemeHttps) });
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    // Cognito access tokens have no audience, the client is checked below.
                    ValidateAudience = false,
                    NameClaimType = CognitoClaims.Username,
                    RoleClaimType = CognitoClaims.Groups
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var principal = context.Principal!;
                        if (principal.FindFirst(CognitoClaims.TokenUse)?.Value != CognitoClaims.AccessTokenUse ||
                            principal.FindFirst(CognitoClaims.ClientId)?.Value != cognito.ClientId)
                        {
                            context.Fail("The token is not an access token of this application.");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        if (string.IsNullOrEmpty(cognito.ServiceUrl))
            services.AddAWSService<IAmazonCognitoIdentityProvider>();
        else
            services.AddSingleton<IAmazonCognitoIdentityProvider>(new AmazonCognitoIdentityProviderClient(
                new AmazonCognitoIdentityProviderConfig { ServiceURL = cognito.ServiceUrl }));

        return services;
    }
}
