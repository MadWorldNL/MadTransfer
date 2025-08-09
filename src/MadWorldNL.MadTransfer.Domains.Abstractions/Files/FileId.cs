namespace MadWorldNL.MadTransfer.Files;

public sealed record FileId(Guid Id)
{
    public static FileId New()
    {
        return new FileId(Guid.NewGuid());
    }
}