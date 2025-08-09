namespace MadWorldNL.MadTransfer.Files.Upload;

public sealed class UserFileDto
{
    public required string Name { get; init; }
    public required long ByteSize { get; init; }
    public required Stream Body { get; init; }
}