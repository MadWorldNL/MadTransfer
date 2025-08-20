namespace MadWorldNL.MadTransfer.Exceptions;

public class InvalidGuidException(string property) : ParseException(ErrorCodes.GuidInvalid, $"The property '{property}' is not a valid guid.")
{
    
}