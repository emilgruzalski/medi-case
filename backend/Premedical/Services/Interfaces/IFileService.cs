namespace Premedical.Services.Interfaces;

public interface IFileService
{
    Task<StoredFile> SaveAsync(IFormFile file);
    Task<StoredFile> SaveAsync(Stream content, string extension);
    Stream OpenRead(string relativePath);
    void Delete(string relativePath);
}
