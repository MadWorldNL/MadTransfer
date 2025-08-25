namespace MadWorldNL.MadTransfer.Status;

public sealed class StatusRepository(MadTransferContext context) : IStatusRepository
{
    public Task<bool> IsAlive() => context.Database.CanConnectAsync();
}