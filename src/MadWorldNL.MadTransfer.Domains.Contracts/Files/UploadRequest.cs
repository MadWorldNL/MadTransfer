using Microsoft.AspNetCore.Http;

namespace MadWorldNL.MadTransfer.Files;

public sealed class UploadRequest
{
    public string Title { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
}