namespace MadWorldNL.MadTransfer.Identities;

public class UserIdNotValidException(string userId) : Exception($"The user id {userId} is not valid.")
{
    public string UserId { get; } = userId;
}