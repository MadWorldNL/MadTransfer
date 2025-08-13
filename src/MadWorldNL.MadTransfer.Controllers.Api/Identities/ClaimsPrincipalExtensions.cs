using System.Security.Claims;

namespace MadWorldNL.MadTransfer.Identities;

internal static class ClaimsPrincipalExtensions
{
    internal static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var identifier =  principal
            .Claims
            .LastOrDefault(c => c.Type is "sub" or ClaimTypes.NameIdentifier)?
            .Value;

        if (!Guid.TryParse(identifier, out var id))
        {
            throw new UserIdNotValidException(identifier ?? "unknown");
        }
        
        return id;
    }
}