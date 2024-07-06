using Amazon.S3;
using MediCaseAPI.Options;
using MediCaseAPI.Services;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Extensions;

public static class FileStorageExtensions
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(FileStorageOptions.SectionName).Get<FileStorageOptions>()
            ?? new FileStorageOptions();

        if (options.Provider != FileStorageProviders.S3)
        {
            services.AddSingleton<IFileStorage, LocalFileStorage>();
            return services;
        }

        if (string.IsNullOrEmpty(options.ServiceUrl))
            services.AddAWSService<IAmazonS3>();
        else
            services.AddSingleton<IAmazonS3>(new AmazonS3Client(new AmazonS3Config
            {
                ServiceURL = options.ServiceUrl,
                ForcePathStyle = true
            }));

        services.AddSingleton<IFileStorage, S3FileStorage>();
        return services;
    }
}
