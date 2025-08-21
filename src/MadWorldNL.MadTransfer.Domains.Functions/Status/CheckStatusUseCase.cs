namespace MadWorldNL.MadTransfer.Status;

public sealed class CheckStatusUseCase(
    IStatusRepository statusRepository,
    IStatusStorage statusStorage)
{
    public async Task<CheckStatusResult> CheckStatus()
    {
        return new CheckStatusResult()
        {
            IsAlive = true,
            IsDatabaseAlive = await statusRepository.IsAlive(),
            IsIdentityServerAlive = false,
            IsStorageAlive = await statusStorage.IsAlive(),
        };
    }
}