using Microsoft.Extensions.Options;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Options;

namespace MediCaseAPI.Services;

public class FileUploadValidator
{
    private readonly FileStorageOptions _options;

    public FileUploadValidator(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    public void Validate(IFormFile file)
    {
        if (file.Length == 0)
            throw new BadRequestException("File is empty.");
        if (file.Length > (long)_options.MaxFileSizeMb * FileStorageOptions.BytesInMegabyte)
            throw new BadRequestException($"File is larger than {_options.MaxFileSizeMb} MB.");
        if (!_options.AllowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            throw new BadRequestException($"Files of type '{file.ContentType}' are not allowed.");
    }
}
