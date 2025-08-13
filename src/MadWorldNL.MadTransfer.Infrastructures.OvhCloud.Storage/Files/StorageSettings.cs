namespace MadWorldNL.MadTransfer.Files;

public sealed class StorageSettings
{
    public const string Key = nameof(StorageSettings);

    public string Host { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string BucketName { get; init; } = string.Empty;
}