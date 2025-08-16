namespace MadWorldNL.MadTransfer.Files.Download;

public sealed class DownloadUserFileResult
{
    public string FileName { get; init; } = string.Empty;
    public Stream FileStream { get; init; } = null!;
}