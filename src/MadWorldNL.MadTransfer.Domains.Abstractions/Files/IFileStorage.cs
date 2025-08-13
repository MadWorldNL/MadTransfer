namespace MadWorldNL.MadTransfer.Files;

public interface IFileStorage
{
    Task Upload(FileMetaData metaData, FilePath path, Stream stream);
}