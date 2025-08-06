namespace MadWorldNL.MadTransfer.Files;

public record FileId(Guid Id)
{
    public static FileId New()
    {
        return new FileId(Guid.NewGuid());
    }
}