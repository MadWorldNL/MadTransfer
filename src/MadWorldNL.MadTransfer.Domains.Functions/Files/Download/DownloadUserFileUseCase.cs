using MadWorldNL.MadTransfer.Exceptions;
using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files.Download;

public sealed class DownloadUserFileUseCase(IFileRepository fileRepository, IFileStorage fileStorage)
{
    private readonly FilePath _path = new("userfiles");
    
    public async Task<Fin<DownloadUserFileResult>> Download(DownloadUserFileCommand command)
    {
        var hyperlinkOutcome = Hyperlink.Create(command.Id);

        var fileInfoOutcome = hyperlinkOutcome.Match(
            GetFileInfo,
            error => error);

        if (fileInfoOutcome.IsFail)
            return fileInfoOutcome.Match(_ => null!, error => error);

        var file = fileInfoOutcome.ThrowIfFail();
        return await RetrieveDownload(file);
    }

    private Fin<UserFile> GetFileInfo(Hyperlink hyperlink)
    {
        var userFileFound = fileRepository.GetUserFile(hyperlink);
        
        return userFileFound.Match(
            userFile => userFile,
            () => Fin<UserFile>.Fail(new NotFoundException(nameof(UserFile))));
    }
    
    private async Task<Fin<DownloadUserFileResult>> RetrieveDownload(UserFile userFile)
    {
        var downloadStreamOutcome = await fileStorage.Download(userFile.MetaData, _path);

        return downloadStreamOutcome.Match(
            stream => Fin<DownloadUserFileResult>.Succ(new DownloadUserFileResult()
            {
                FileName = userFile.MetaData.Name,
                FileStream = stream
            }),
            error => error
        );
    }
}