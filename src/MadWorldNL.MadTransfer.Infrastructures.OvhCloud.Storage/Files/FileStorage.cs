using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using LanguageExt;
using MadWorldNL.MadTransfer.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MadWorldNL.MadTransfer.Files;

public sealed class FileStorage(IOptions<StorageSettings> settings, ILogger<FileStorage> logger) : IFileStorage
{
    private readonly StorageSettings _settings = settings.Value;
    private readonly AmazonS3Client _client = settings.Value.GetClient();

    public async Task Upload(FileMetaData metaData, FilePath path, Stream stream)
    {
        stream.Position = 0;

        var putRequest = new PutObjectRequest()
        {
            BucketName = _settings.BucketName,
            Key = $"{path.Value}/{metaData.InternalName}",
            InputStream = stream,
            ContentType = "application/octet-stream"
        };

        try
        {
            var response = await _client.PutObjectAsync(putRequest);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return; // Return the key to access the file.
            }
        }
        catch (AmazonS3Exception exception)
        {
            logger.LogError(exception, "There was an error uploading the file");
        }
    }

    public async Task<Fin<Stream>> Download(FileMetaData metaData, FilePath path)
    {
        var getRequest = new GetObjectRequest()
        {
            BucketName = _settings.BucketName,
            Key = $"{path.Value}/{metaData.InternalName}",
        };
        
        try
        {
            var response = await _client.GetObjectAsync(getRequest);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return response.ResponseStream;
            }
        }
        catch (AmazonS3Exception exception)
        {
            logger.LogError(exception, "There was an error downloading the file");
        }
        
        return Fin<Stream>.Fail(new NotFoundException(nameof(UserFile)));   
    }
}