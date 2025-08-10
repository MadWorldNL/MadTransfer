using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files.Upload;

public sealed class UploadUserFileUseCase(IFileRepository fileRepository, IFileStorage fileStorage)
{
    private readonly FilePath _path = new("userfiles");
    
    public async Task<Fin<UploadUserFileResult>> Upload(UploadUserFileCommand command)
    {
        var fileOutcome = CreateUserFile(command);

        if (fileOutcome.IsFail)
            return fileOutcome.Match(_ => null!, error => error);

        var file = fileOutcome.ThrowIfFail();
        return await ExecuteUpload(file, command.File);
    }

    private async Task<UploadUserFileResult> ExecuteUpload(UserFile file, UserFileDto fileDto)
    {
        await fileRepository.Add(file);
        await fileStorage.Upload(file.MetaData, _path, fileDto.Body);
        
        return new UploadUserFileResult()
        {
            Url = file.Url.Value
        };
    }

    private static Fin<UserFile> CreateUserFile(UploadUserFileCommand command)
    {
        var extension = Path.GetExtension(command.File.Name);
        var metaDataOutcome = FileMetaData.Create(command.File.Name, Guid.NewGuid().ToString(), extension, command.File.ByteSize);

        return metaDataOutcome.Match(
            metaData => Fin<UserFile>.Succ(new UserFile(FileId.New(), metaData, Hyperlink.Create(Guid.NewGuid()), new UserId(command.UserId))),
            error => error
        );
    }
}