using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files;

public sealed class FileRepository(MadTransferContext context) : IFileRepository
{
    public void Add(UserFile userFile)
    {
        var fileFound = GetUserFile(userFile.Url);

        if (fileFound != null)
        {
            // TODO: Add better exception
            throw new Exception();
        }
        
        context.Add(userFile);
        context.SaveChanges();
    }

    private UserFile? GetUserFile(Hyperlink url)
    {
        return context.Files.FirstOrDefault(f => f.Url.Equals(url));
    }
}