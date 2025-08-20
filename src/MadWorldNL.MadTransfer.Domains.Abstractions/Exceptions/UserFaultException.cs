namespace MadWorldNL.MadTransfer.Exceptions;

public class UserFaultException(ErrorCodes errorCode, string message) : Exception(message)
{
    public ErrorCodes ErrorCode { get; } = errorCode;
}