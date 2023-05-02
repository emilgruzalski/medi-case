using Microsoft.Extensions.Options;
using Premedical.Exceptions;
using Premedical.Options;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

public class FileService : IFileService
{
    private readonly FileStorageOptions _options;
    private readonly string _rootPath;

    public FileService(IOptions<FileStorageOptions> options, IWebHostEnvironment environment)
    {
        _options = options.Value;
        _rootPath = Path.GetFullPath(Path.IsPathRooted(_options.RootPath)
            ? _options.RootPath
            : Path.Combine(environment.ContentRootPath, _options.RootPath));
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<StoredFile> SaveAsync(IFormFile file)
    {
        if (file.Length == 0)
            throw new BadRequestException("File is empty.");
        if (file.Length > (long)_options.MaxFileSizeMb * FileStorageOptions.BytesInMegabyte)
            throw new BadRequestException($"File is larger than {_options.MaxFileSizeMb} MB.");
        if (!_options.AllowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            throw new BadRequestException($"Files of type '{file.ContentType}' are not allowed.");

        await using var stream = file.OpenReadStream();
        return await SaveAsync(stream, Path.GetExtension(file.FileName));
    }

    public async Task<StoredFile> SaveAsync(Stream content, string extension)
    {
        var relativePath = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var fullPath = ResolvePath(relativePath);

        // The upload is buffered to disk in one go; files are small (audio clips and images).
        await using (var target = File.Create(fullPath))
        {
            await content.CopyToAsync(target);
        }

        return new StoredFile(relativePath, new FileInfo(fullPath).Length);
    }

    public Stream OpenRead(string relativePath)
    {
        var fullPath = ResolvePath(relativePath);
        if (!File.Exists(fullPath))
            throw new NotFoundException("File does not exist on the server.");

        return File.OpenRead(fullPath);
    }

    public void Delete(string relativePath)
    {
        var fullPath = ResolvePath(relativePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public int DeleteUnusedFiles(IReadOnlySet<string> usedPaths)
    {
        // Recently written files may belong to an upload that is not saved in the database yet.
        var threshold = DateTime.UtcNow.AddMinutes(-_options.UnusedFileGracePeriodMinutes);
        var unused = new DirectoryInfo(_rootPath)
            .EnumerateFiles()
            .Where(f => f.LastWriteTimeUtc < threshold && !usedPaths.Contains(f.Name))
            .ToList();

        foreach (var file in unused)
            file.Delete();

        return unused.Count;
    }

    private string ResolvePath(string relativePath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(_rootPath, relativePath));
        if (!fullPath.StartsWith(_rootPath, StringComparison.Ordinal))
            throw new BadRequestException("Invalid file path.");

        return fullPath;
    }
}
