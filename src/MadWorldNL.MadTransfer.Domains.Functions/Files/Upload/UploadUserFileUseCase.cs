namespace MadWorldNL.MadTransfer.Files.Upload;

public class UploadUserFileUseCase
{
    public UploadUserFileResult Upload(UploadUserFileCommand command)
    {
        return new UploadUserFileResult()
        {
            Url = "example.nl/download"
        };
    }
}