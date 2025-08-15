using MadWorldNL.MadTransfer.Files;
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
            .DisableAntiforgery();

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

        endpoints.MapGet("/Info", ([AsParameters] InfoRequest request, [FromServices] GetInfoUserFileUseCase useCase)  =>
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
        
        endpoints.MapGet("/Download", ([AsParameters] DownloadRequest request) =>
        {
            var text = $"Hello from the server! ({request.Id})";
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            var stream = new MemoryStream(bytes);
            
            var provider = new FileExtensionContentTypeProvider();

            const string defaultContentType = "application/octet-stream";
            if (!provider.TryGetContentType("example.txt", out string? contentType))
            {
                contentType = defaultContentType;
            }

            return Results.File(stream, contentType, "example.txt");
        });
    }
}