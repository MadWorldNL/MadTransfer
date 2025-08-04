using System.Security.Claims;

namespace MadWorldNL.MadTransfer.Identities;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var identifier =  principal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (!Guid.TryParse(identifier, out var id))
        {
            
        }
        
        return id;
    }
}