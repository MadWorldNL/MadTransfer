using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace MadWorldNL.MadTransfer.Files;

public class FileStorage : IFileStorage
{
    public async Task Upload(FileMetaData metaData, FilePath path, Stream stream)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = "http://localhost:9444",
            ForcePathStyle = true // Important for S3-compatible storage!
        };
        var client = new AmazonS3Client(new BasicAWSCredentials("AKIAIOSFODNN7EXAMPLE", "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"), config);
        
        stream.Position = 0;
        var putRequest = new PutObjectRequest
        {
            BucketName = path.Value,
            Key = metaData.InternalName,
            InputStream = stream,
            ContentType = "application/octet-stream"
        };
        
        var response = await client.PutObjectAsync(putRequest);

        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return;  // Return the key to access the file.
        }
        
        throw new NotImplementedException();
    }
}