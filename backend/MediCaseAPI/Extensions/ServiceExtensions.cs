using MediCaseAPI.Repositories;
using MediCaseAPI.Repositories.Interfaces;
using MediCaseAPI.Seeders;
using MediCaseAPI.Services;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEntityTypeRepository, EntityTypeRepository>();
        services.AddScoped<IEntityLanguageRepository, EntityLanguageRepository>();
        services.AddScoped<IEntityRepository, EntityRepository>();
        services.AddScoped<IEntityTranslationsRepository, EntityTranslationsRepository>();
        services.AddScoped<IEntityGraphDataRepository, EntityGraphDataRepository>();
        services.AddScoped<IEntityDictionaryRepository, EntityDictionaryRepository>();
        services.AddScoped<IEntityTranslationFilesRepository, EntityTranslationFilesRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICognitoUserAdmin, CognitoUserAdmin>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IEntityLanguageService, EntityLanguageService>();
        services.AddScoped<IEntityTranslationService, EntityTranslationService>();
        services.AddScoped<IEntityDictionaryService, EntityDictionaryService>();
        services.AddScoped<IEntityService, EntityService>();
        services.AddSingleton<FileUploadValidator>();
        services.AddScoped<IEntityTranslationFileService, EntityTranslationFileService>();
        services.AddScoped<ISynchronizationService, SynchronizationService>();
        services.AddScoped<ILearningService, LearningService>();
        services.AddScoped<ITextTranslator, AwsTextTranslator>();
        services.AddScoped<ISpeechSynthesizer, AwsSpeechSynthesizer>();
        services.AddScoped<IAiContentService, AiContentService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<DataSeeder>();
        services.AddScoped<SampleContentSeeder>();
        services.AddScoped<CognitoAdminSeeder>();
        return services;
    }
}
