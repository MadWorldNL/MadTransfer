namespace MadWorldNL.MadTransfer.Configurations;

public class AuthenticationSettings
{
    public const string Key = "Authentication";

    public string Authority { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public bool ValidateUser { get; init; }
}
