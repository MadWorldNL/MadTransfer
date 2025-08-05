using Microsoft.AspNetCore.Http;

namespace MadWorldNL.MadTransfer.Files;

public sealed class UploadRequest
{
    public IFormFile File { get; set; } = null!;
}