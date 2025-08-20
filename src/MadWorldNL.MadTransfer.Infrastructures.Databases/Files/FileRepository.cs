using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files;

public sealed class FileRepository(MadTransferContext context) : IFileRepository
{
    public async Task Add(UserFile userFile)
    {
        var fileFound = GetUserFile(userFile.Url);

        if (fileFound.IsSome)
        {
            throw new DataBaseEntryDuplicatedException(nameof(UserFile), nameof(userFile.Url), userFile.Url.Value);
        }
        
        await context.AddAsync(userFile);
        await context.SaveChangesAsync();
    }

    public Option<UserFile> GetUserFile(Hyperlink url)
    {
        return context.Files.Find(f => f.Url.Equals(url));
    }
}