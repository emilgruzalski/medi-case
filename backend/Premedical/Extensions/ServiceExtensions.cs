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
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
