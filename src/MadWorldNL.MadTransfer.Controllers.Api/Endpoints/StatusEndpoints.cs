using MadWorldNL.MadTransfer.Status;
using Microsoft.AspNetCore.Mvc;

namespace MadWorldNL.MadTransfer.Endpoints;

internal static class StatusEndpoints
{
    internal static void AddStatusEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("/Status");

        endpoints.MapGet("/", async ([FromServices] CheckStatusUseCase useCase) =>
        {
            var result = await useCase.CheckStatus();
            
            return new StatusResponse()
            {
                IsAlive = result.IsAlive,
                IsDatabaseAlive = result.IsDatabaseAlive,
                IsIdentityServerAlive = result.IsIdentityServerAlive,
                IsStorageAlive = result.IsStorageAlive
            };
        });
    }
}