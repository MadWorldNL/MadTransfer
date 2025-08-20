using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files;

public interface IFileStorage
{
    Task Upload(FileMetaData metaData, FilePath path, Stream stream);
    Task<Fin<Stream>> Download(FileMetaData metaData, FilePath path);
}