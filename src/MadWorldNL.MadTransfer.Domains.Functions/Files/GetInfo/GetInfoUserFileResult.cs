namespace MadWorldNL.MadTransfer.Files.GetInfo;

public sealed class GetInfoUserFileResult
{
    public string Id { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public float FileSize { get; init; }
}