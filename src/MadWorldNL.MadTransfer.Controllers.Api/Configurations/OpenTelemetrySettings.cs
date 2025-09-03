namespace MadWorldNL.MadTransfer.Configurations;

public sealed class OpenTelemetrySettings
{
    public const string Key = nameof(OpenTelemetrySettings);
    
    public OpenTelemetryMode Mode { get; init; } = OpenTelemetryMode.None;
    public SeqSettings Seq { get; init; } = new();
}