using MadWorldNL.MadTransfer.Files;
using Microsoft.AspNetCore.Mvc;

namespace MadWorldNL.MadTransfer.Endpoints;

internal static class FileEndpoints
{
    internal static void AddFileEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("/File")
            .DisableAntiforgery();

        endpoints.MapPost("/Upload", ([FromForm] UploadRequest request) =>
        {
            return Results.Accepted();
        });
    }
}