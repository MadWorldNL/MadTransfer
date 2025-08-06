using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files.Upload;

public class UploadUserFileUseCase(IFileRepository fileRepository, IFileStorage fileStorage)
{
    private readonly FilePath _path = new("userfiles");
    
    public UploadUserFileResult Upload(UploadUserFileCommand command)
    {
        var extension = Path.GetExtension(command.File.Name);
        var metaData = FileMetaData.Create(command.File.Name, Guid.NewGuid().ToString(), extension, (int)command.File.ByteSize);
        var file = new UserFile(FileId.New(), metaData, Hyperlink.Create(metaData.InternalName), new UserId(command.UserId));
        
        fileRepository.Add(file);
        fileStorage.Upload(file.MetaData, _path, command.File.Body);
        
        return new UploadUserFileResult()
        {
            Url = "example.nl/download"
        };
    }
}