namespace MadWorldNL.MadTransfer.Exceptions;

public sealed class NegativeException(string property) : ParseException(ErrorCodes.Negative, $"The property '{property}' cannot be negative.")
{
    
}