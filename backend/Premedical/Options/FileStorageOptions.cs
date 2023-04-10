namespace Premedical.Options;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";
    public const int BytesInMegabyte = 1024 * 1024;

    public string RootPath { get; set; } = null!;
    public int MaxFileSizeMb { get; set; }
    public string[] AllowedContentTypes { get; set; } = Array.Empty<string>();
}
