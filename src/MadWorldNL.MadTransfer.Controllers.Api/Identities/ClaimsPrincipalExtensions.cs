using System.Security.Claims;

namespace MadWorldNL.MadTransfer.Identities;

internal static class ClaimsPrincipalExtensions
{
    internal static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var identifier =  principal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (!Guid.TryParse(identifier, out var id))
        {
            
        }
        
        return id;
    }
}