namespace MadWorldNL.MadTransfer.Status;

public interface IStatusRepository
{
    Task<bool> IsAlive();
}