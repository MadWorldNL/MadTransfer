namespace MadWorldNL.MadTransfer.Files;

public sealed class InfoResponse
{
    public string Id { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public float FileSize { get; set; }
}