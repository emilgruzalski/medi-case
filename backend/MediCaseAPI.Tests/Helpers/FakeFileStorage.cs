using MediCaseAPI.Services;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Tests.Helpers;

public class FakeFileStorage : IFileStorage
{
    public IReadOnlySet<string>? LastUsedPaths { get; private set; }

    public Task<StoredFile> SaveAsync(Stream content, string extension, string contentType) =>
        throw new NotSupportedException();

    public Task<Stream> OpenReadAsync(string relativePath) => throw new NotSupportedException();

    public Task<int> DeleteUnusedFilesAsync(IReadOnlySet<string> usedPaths)
    {
        LastUsedPaths = usedPaths;
        return Task.FromResult(0);
    }
}
