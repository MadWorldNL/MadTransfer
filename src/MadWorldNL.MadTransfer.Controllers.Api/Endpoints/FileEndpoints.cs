using MadWorldNL.MadTransfer.Configurations;
using MadWorldNL.MadTransfer.Files;
using MadWorldNL.MadTransfer.Files.Download;
using MadWorldNL.MadTransfer.Files.GetInfo;
using MadWorldNL.MadTransfer.Files.Upload;
using MadWorldNL.MadTransfer.Identities;
using MadWorldNL.MadTransfer.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MadWorldNL.MadTransfer.Endpoints;

internal static class FileEndpoints
{
    internal static void AddFileEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("/File")
            .DisableAntiforgery()
            .RequireAuthorization()
            .RequireRateLimiting(RateLimiterNames.PerUserPolicy);

        endpoints.MapPost("/Upload",
            async ([FromForm] UploadRequest request,
                HttpContext httpContext,
                [FromServices] UploadUserFileUseCase useCase) =>
            {
                var userId = httpContext.User.GetUserId();

                var command = new UploadUserFileCommand()
                {
                    File = new UserFileDto()
                    {
                        Name = request.File.FileName,
                        ByteSize = request.File.Length,
                        Body = request.File.OpenReadStream()
                    },
                    UserId = userId
                };

                var uploadOutcome = await useCase.Upload(command);

                return uploadOutcome.Match(
                    result => Results.Ok(new UploadResponse()
                    {
                        DownloadUrl = $"/File/Download/{result.Url}"
                    }), error => error.ToFaultyResult());
            }).RequireAuthorization();

        endpoints.MapGet("/Info", ([AsParameters] InfoRequest request, 
            [FromServices] GetInfoUserFileUseCase useCase) =>
        {
            var command = new GetInfoUserFileCommand()
            {
                Id = request.Id
            };

            var getInfoOutcome = useCase.GetInfo(command);

            return getInfoOutcome.Match(result => Results.Ok(new InfoResponse()
            {
                Id = result.Id,
                FileName = result.FileName,
                FileSize = result.FileSize
            }), error => error.ToFaultyResult());
        });
        
        endpoints.MapGet("/Download", async ([AsParameters] DownloadRequest request, 
            [FromServices] DownloadUserFileUseCase useCase) =>
        {
            var command = new DownloadUserFileCommand()
            {
                Id = request.Id
            };

            var downloadOutcome = await useCase.Download(command);
            return downloadOutcome.Match(
                CreateFileResponse, 
                error => error.ToFaultyResult());
        });
    }

    private static IResult CreateFileResponse(DownloadUserFileResult download)
    {
        var provider = new FileExtensionContentTypeProvider();

        const string defaultContentType = "application/octet-stream";
        if (!provider.TryGetContentType(download.FileName, out var contentType))
        {
            contentType = defaultContentType;
        }
        
        return Results.File(download.FileStream, contentType, download.FileName);
    }
}