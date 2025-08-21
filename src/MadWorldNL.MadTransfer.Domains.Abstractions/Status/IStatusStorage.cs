namespace MadWorldNL.MadTransfer.Status;

public interface IStatusStorage
{
    Task<bool> IsAlive();
}