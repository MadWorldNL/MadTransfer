using System.Net;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace MadWorldNL.MadTransfer.Status;

public sealed class StatusStorage(IOptions<StorageSettings> settings) : IStatusStorage
{
    private readonly AmazonS3Client _client = settings.Value.GetClient();
    
    public async Task<bool> IsAlive()
    {
        try
        {
            var response = await _client.ListBucketsAsync();
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }
}