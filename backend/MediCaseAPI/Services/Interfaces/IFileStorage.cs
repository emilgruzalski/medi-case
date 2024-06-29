namespace MediCaseAPI.Services.Interfaces;

public interface IFileStorage
{
    Task<StoredFile> SaveAsync(Stream content, string extension, string contentType);
    Task<Stream> OpenReadAsync(string relativePath);

    /// <returns>Number of removed files.</returns>
    Task<int> DeleteUnusedFilesAsync(IReadOnlySet<string> usedPaths);
}
