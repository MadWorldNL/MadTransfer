using MadWorldNL.MadTransfer.Files;
using MadWorldNL.MadTransfer.Identities;
using Microsoft.AspNetCore.Mvc;

namespace MadWorldNL.MadTransfer.Endpoints;

internal static class FileEndpoints
{
    // https://hub.docker.com/r/openstackswift/saio
    
    internal static void AddFileEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("/File")
            .DisableAntiforgery();

        endpoints.MapPost("/Upload", ([FromForm] UploadRequest request, HttpContext httpContext) =>
            {
                var userId = httpContext.User.GetUserId();

                return Results.Ok(new UploadResponse()
                {
                    DownloadUrl = "example.nl/download"
                });
            })
            .RequireAuthorization();
    }
}