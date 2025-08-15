using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files;

public interface IFileRepository
{
    Task Add(UserFile userFile);
    Option<UserFile> GetUserFile(Hyperlink url);
}