namespace MadWorldNL.MadTransfer.Exceptions;

public class EmptyException(string property) : ParseException(ErrorCodes.Empty, $"The property '{property}' cannot be empty.")
{
}