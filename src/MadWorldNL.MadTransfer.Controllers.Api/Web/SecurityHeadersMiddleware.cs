namespace MadWorldNL.MadTransfer.Web;

public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains; preload";
        context.Response.Headers.XContentTypeOptions = "nosniff";
        context.Response.Headers.XFrameOptions = "DENY";
        context.Response.Headers["Referrer-Policy"] = "same-origin";
        context.Response.Headers.ContentSecurityPolicy = "default-src 'self'";
        context.Response.Headers["Permissions-Policy"] = "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()";
        
        await next(context);
    }
}