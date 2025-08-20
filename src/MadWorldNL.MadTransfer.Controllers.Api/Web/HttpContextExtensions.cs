namespace MadWorldNL.MadTransfer.Web;

public static class HttpContextExtensions
{
    public static string GetIpAddress(this HttpContext httpContext)
    {
        // Try to get the IP from the X-Forwarded-For header
        var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        // If not available, fall back to RemoteIpAddress
        if (string.IsNullOrEmpty(ip))
        {
            ip = httpContext.Connection.RemoteIpAddress?.ToString();
        }

        return ip ?? "unknown";
    }
}