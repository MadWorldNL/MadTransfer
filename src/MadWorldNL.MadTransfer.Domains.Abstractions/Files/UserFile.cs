using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;
using NodaTime;

namespace MadWorldNL.MadTransfer.Files;

public sealed class UserFile
{
    public FileId Id { get; init; } = null!;
    public FileMetaData MetaData { get; init; } = null!;
    public Hyperlink Url { get; init; } = null!;
    public UserId UserId { get; init; } = null!;
    public Instant CreatedAt { get; init; }

    public UserFile(FileId id, FileMetaData metaData, Hyperlink url, UserId userId)
    {
        Id = id;
        MetaData = metaData;
        Url = url;
        UserId = userId;
        
        CreatedAt = SystemClock.Instance.GetCurrentInstant();
    }
    
    /// <summary>
    /// Only used by entity framework
    /// </summary>
    [UsedImplicitly]
    private UserFile()
    {
    }
}