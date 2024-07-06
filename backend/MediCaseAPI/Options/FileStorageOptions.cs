namespace MediCaseAPI.Options;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";
    public const int BytesInMegabyte = 1024 * 1024;

    public string Provider { get; set; } = FileStorageProviders.Local;
    public string RootPath { get; set; } = null!;
    public string? BucketName { get; set; }
    public string KeyPrefix { get; set; } = string.Empty;

    /// <summary>Endpoint of an S3 compatible storage (e.g. MinIO) used instead of Amazon S3.</summary>
    public string? ServiceUrl { get; set; }

    public int MaxFileSizeMb { get; set; }
    public string[] AllowedContentTypes { get; set; } = Array.Empty<string>();
    public int UnusedFileGracePeriodMinutes { get; set; }
}
