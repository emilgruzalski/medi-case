using Microsoft.AspNetCore.Http;
using MediCaseAPI.Services;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Tests.Helpers;

public class FakeFileService : IFileService
{
    public IReadOnlySet<string>? LastUsedPaths { get; private set; }

    public Task<StoredFile> SaveAsync(IFormFile file) => throw new NotSupportedException();

    public Task<StoredFile> SaveAsync(Stream content, string extension) => throw new NotSupportedException();

    public Stream OpenRead(string relativePath) => throw new NotSupportedException();

    public void Delete(string relativePath)
    {
    }

    public int DeleteUnusedFiles(IReadOnlySet<string> usedPaths)
    {
        LastUsedPaths = usedPaths;
        return 0;
    }
}
