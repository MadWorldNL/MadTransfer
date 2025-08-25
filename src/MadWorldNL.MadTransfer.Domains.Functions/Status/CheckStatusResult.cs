namespace MadWorldNL.MadTransfer.Status;

public class CheckStatusResult
{
    public bool IsAlive { get; set; }
    public bool IsDatabaseAlive { get; set; }
    public bool IsIdentityServerAlive { get; set; }
    public bool IsStorageAlive { get; set; }
}