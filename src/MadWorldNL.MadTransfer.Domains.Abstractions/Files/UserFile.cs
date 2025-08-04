using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Files;

public class UserFile
{
    public required FileId Id { get; init; }
    public required Hyperlink Url { get; init; }
    public required UserId UserId { get; init; }
}