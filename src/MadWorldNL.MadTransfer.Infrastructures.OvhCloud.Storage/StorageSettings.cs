using Amazon.Runtime;
using Amazon.S3;

namespace MadWorldNL.MadTransfer;

public sealed class StorageSettings
{
    public const string Key = nameof(StorageSettings);

    public string Host { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string BucketName { get; init; } = string.Empty;
    
    public AmazonS3Client GetClient()
    {
        var config = new AmazonS3Config
        {
            ServiceURL = Host,
            ForcePathStyle = true
        };

        return new AmazonS3Client(new BasicAWSCredentials(AccessKey, SecretKey), config);  
    }
}