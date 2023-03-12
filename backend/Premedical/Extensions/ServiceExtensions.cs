using Premedical.Repositories;
using Premedical.Repositories.Interfaces;
using Premedical.Services;
using Premedical.Services.Interfaces;

namespace Premedical.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IEntityTypeRepository, EntityTypeRepository>();
        services.AddScoped<IEntityLanguageRepository, EntityLanguageRepository>();
        services.AddScoped<IEntityRepository, EntityRepository>();
        services.AddScoped<IEntityTranslationsRepository, EntityTranslationsRepository>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IEntityLanguageService, EntityLanguageService>();
        services.AddScoped<IEntityTranslationService, EntityTranslationService>();
        return services;
    }
}
