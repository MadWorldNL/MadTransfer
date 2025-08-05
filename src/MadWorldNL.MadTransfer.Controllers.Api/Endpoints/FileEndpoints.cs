using MadWorldNL.MadTransfer.Files;
using MadWorldNL.MadTransfer.Files.Upload;
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

        endpoints.MapPost("/Upload",
                ([FromForm] UploadRequest request,
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

                    var result = useCase.Upload(command);

                    return Results.Ok(new UploadResponse()
                    {
                        DownloadUrl = result.Url
                    });
                }).RequireAuthorization();
    }
}