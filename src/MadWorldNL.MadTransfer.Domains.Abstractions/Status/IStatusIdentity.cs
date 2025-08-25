namespace MadWorldNL.MadTransfer.Status;

public interface IStatusIdentity
{
    Task<bool> IsAlive();
}