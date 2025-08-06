using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files.Upload;

public class UploadUserFileUseCase(IFileRepository fileRepository, IFileStorage fileStorage)
{
    private readonly FilePath _path = new("userfiles");
    
    public async Task<UploadUserFileResult> Upload(UploadUserFileCommand command)
    {
        var extension = Path.GetExtension(command.File.Name);
        var metaData = FileMetaData.Create(command.File.Name, Guid.NewGuid().ToString(), extension, command.File.ByteSize);
        var file = new UserFile(FileId.New(), metaData, Hyperlink.Create(Guid.NewGuid()), new UserId(command.UserId));
        
        fileRepository.Add(file);
        await fileStorage.Upload(file.MetaData, _path, command.File.Body);
        
        return new UploadUserFileResult()
        {
            Url = "example.nl/download"
        };
    }
}