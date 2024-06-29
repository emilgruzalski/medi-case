namespace MediCaseAPI.Services;

public static class StoredFileName
{
    public static string Create(string extension) => $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
}
