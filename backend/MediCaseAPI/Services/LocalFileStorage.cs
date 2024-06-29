using Microsoft.Extensions.Options;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Options;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

/// <summary>Stores files on the local disk, used for development.</summary>
public class LocalFileStorage : IFileStorage
{
    private readonly FileStorageOptions _options;
    private readonly string _rootPath;

    public LocalFileStorage(IOptions<FileStorageOptions> options, IWebHostEnvironment environment)
    {
        _options = options.Value;
        _rootPath = Path.GetFullPath(Path.IsPathRooted(_options.RootPath)
            ? _options.RootPath
            : Path.Combine(environment.ContentRootPath, _options.RootPath));
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<StoredFile> SaveAsync(Stream content, string extension, string contentType)
    {
        var relativePath = StoredFileName.Create(extension);
        var fullPath = ResolvePath(relativePath);

        await using (var target = File.Create(fullPath))
        {
            await content.CopyToAsync(target);
        }

        return new StoredFile(relativePath, new FileInfo(fullPath).Length);
    }

    public Task<Stream> OpenReadAsync(string relativePath)
    {
        var fullPath = ResolvePath(relativePath);
        if (!File.Exists(fullPath))
            throw new NotFoundException("File does not exist on the server.");

        return Task.FromResult<Stream>(File.OpenRead(fullPath));
    }

    public Task<int> DeleteUnusedFilesAsync(IReadOnlySet<string> usedPaths)
    {
        // Recently written files may belong to an upload that is not saved in the database yet.
        var threshold = DateTime.UtcNow.AddMinutes(-_options.UnusedFileGracePeriodMinutes);
        var unused = new DirectoryInfo(_rootPath)
            .EnumerateFiles()
            .Where(f => f.LastWriteTimeUtc < threshold && !usedPaths.Contains(f.Name))
            .ToList();

        foreach (var file in unused)
            file.Delete();

        return Task.FromResult(unused.Count);
    }

    private string ResolvePath(string relativePath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(_rootPath, relativePath));
        if (!fullPath.StartsWith(_rootPath, StringComparison.Ordinal))
            throw new BadRequestException("Invalid file path.");

        return fullPath;
    }
}
