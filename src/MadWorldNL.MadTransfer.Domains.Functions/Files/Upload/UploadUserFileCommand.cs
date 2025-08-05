namespace MadWorldNL.MadTransfer.Files.Upload;

public class UploadUserFileCommand
{
    public required UserFileDto File { get; init; }
    public required Guid UserId { get; init; }
}