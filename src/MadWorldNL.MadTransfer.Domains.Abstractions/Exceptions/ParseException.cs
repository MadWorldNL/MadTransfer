namespace MadWorldNL.MadTransfer.Exceptions;

public class ParseException(ErrorCodes errorCode, string message) : Exception(message)
{
    public ErrorCodes ErrorCode { get; } = errorCode;
}