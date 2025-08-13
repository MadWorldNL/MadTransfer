namespace MadWorldNL.MadTransfer.Files.Upload;

public sealed class UploadUserFileCommand
{
    public required UserFileDto File { get; init; }
    public required Guid UserId { get; init; }
}