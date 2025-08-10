namespace MadWorldNL.MadTransfer.Files;

public interface IFileRepository
{
    Task Add(UserFile userFile);
}