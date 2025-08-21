using System.Net;
using MadWorldNL.MadTransfer.Configurations;
using MadWorldNL.MadTransfer.Status;
using Microsoft.Extensions.Options;

namespace MadWorldNL.MadTransfer;

public class StatusIdentity(HttpClient httpClient, IOptions<AuthenticationSettings> settings) : IStatusIdentity
{
    private readonly AuthenticationSettings _settings = settings.Value;
    
    public async Task<bool> IsAlive()
    {
        var response = await httpClient.GetAsync(_settings.Authority);
        return response.StatusCode == HttpStatusCode.OK;
    }
}