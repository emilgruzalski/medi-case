using Microsoft.OpenApi.Models;
using Premedical.Authentication;

namespace Premedical.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithSessionAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(SessionAuthenticationDefaults.Scheme, new OpenApiSecurityScheme
            {
                Description = "Session token returned by POST /api/Sessions/login",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SessionAuthenticationDefaults.Scheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
