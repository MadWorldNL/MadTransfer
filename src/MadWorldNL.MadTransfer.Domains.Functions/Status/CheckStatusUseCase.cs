namespace MadWorldNL.MadTransfer.Status;

public sealed class CheckStatusUseCase(
    IStatusRepository statusRepository,
    IStatusIdentity statusIdentity,
    IStatusStorage statusStorage)
{
    public async Task<CheckStatusResult> CheckStatus()
    {
        return new CheckStatusResult()
        {
            IsAlive = true,
            IsDatabaseAlive = await statusRepository.IsAlive(),
            IsIdentityServerAlive = await statusIdentity.IsAlive(),
            IsStorageAlive = await statusStorage.IsAlive(),
        };
    }
}