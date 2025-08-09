using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace MadWorldNL.MadTransfer.Files;

public sealed class FileStorage(IOptions<StorageSettings> settings) : IFileStorage
{
    private readonly StorageSettings _settings = settings.Value;
    private readonly AmazonS3Client _client = GetClient(settings.Value);

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
        catch (Exception ex)
        {
            _ = ex;
        }
        
        throw new NotImplementedException();
    }

    private static AmazonS3Client GetClient(StorageSettings settings)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = settings.Host,
            ForcePathStyle = true
        };

        return new AmazonS3Client(new BasicAWSCredentials(settings.AccessKey, settings.SecretKey), config);  
    }
}