using MadWorldNL.MadTransfer.Exceptions;
using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files.GetInfo;

public sealed class GetInfoUserFileUseCase(IFileRepository fileRepository)
{
    public Fin<GetInfoUserFileResult> GetInfo(GetInfoUserFileCommand command)
    {
        var hyperlinkOutcome = Hyperlink.Create(command.Id);

        return hyperlinkOutcome.Match(
            GetFileInfo,
            error => error);
    }

    private Fin<GetInfoUserFileResult> GetFileInfo(Hyperlink hyperlink)
    {
        var userFileFound = fileRepository.GetUserFile(hyperlink);
        
        return userFileFound.Match(userFile => new GetInfoUserFileResult()
            {
                Id = hyperlink.Value,
                FileName = userFile.MetaData.Name
            },
            () => Fin<GetInfoUserFileResult>.Fail(new NotFoundException(nameof(UserFile))));
    }
}