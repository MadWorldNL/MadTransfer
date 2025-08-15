namespace MadWorldNL.MadTransfer.Exceptions;

public sealed class NotFoundException(string objectType) : UserFaultException(ErrorCodes.NotFound, $"The '{objectType}' was not found.")
{
    
}