using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Options;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

/// <summary>Stores files in an Amazon S3 bucket.</summary>
public class S3FileStorage : IFileStorage
{
    // Limit of a single DeleteObjects request.
    private const int MaxKeysPerDelete = 1000;

    private readonly IAmazonS3 _s3;
    private readonly FileStorageOptions _options;
    private readonly string _bucketName;

    public S3FileStorage(IAmazonS3 s3, IOptions<FileStorageOptions> options)
    {
        _s3 = s3;
        _options = options.Value;
        _bucketName = _options.BucketName
            ?? throw new InvalidOperationException("FileStorage:BucketName is required for the S3 provider.");
    }

    public async Task<StoredFile> SaveAsync(Stream content, string extension, string contentType)
    {
        var relativePath = StoredFileName.Create(extension);

        // Files are small (audio clips, images), so they are buffered to send the content length to S3.
        await using var buffer = new MemoryStream();
        await content.CopyToAsync(buffer);
        buffer.Position = 0;

        await _s3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = ToKey(relativePath),
            InputStream = buffer,
            ContentType = contentType,
            AutoCloseStream = false
        });

        return new StoredFile(relativePath, buffer.Length);
    }

    public async Task<Stream> OpenReadAsync(string relativePath)
    {
        try
        {
            var response = await _s3.GetObjectAsync(_bucketName, ToKey(relativePath));
            return response.ResponseStream;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException("File does not exist in the storage.");
        }
    }

    public async Task<int> DeleteUnusedFilesAsync(IReadOnlySet<string> usedPaths)
    {
        var threshold = DateTime.UtcNow.AddMinutes(-_options.UnusedFileGracePeriodMinutes);
        var unusedKeys = new List<string>();

        var paginator = _s3.Paginators.ListObjectsV2(new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = _options.KeyPrefix
        });
        await foreach (var item in paginator.S3Objects)
        {
            var relativePath = item.Key[_options.KeyPrefix.Length..];
            if (item.LastModified.ToUniversalTime() < threshold && !usedPaths.Contains(relativePath))
                unusedKeys.Add(item.Key);
        }

        foreach (var chunk in unusedKeys.Chunk(MaxKeysPerDelete))
        {
            await _s3.DeleteObjectsAsync(new DeleteObjectsRequest
            {
                BucketName = _bucketName,
                Objects = chunk.Select(key => new KeyVersion { Key = key }).ToList()
            });
        }

        return unusedKeys.Count;
    }

    private string ToKey(string relativePath) => _options.KeyPrefix + relativePath;
}
