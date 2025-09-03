using System.Threading.RateLimiting;
using MadWorldNL.MadTransfer.Configurations;
using MadWorldNL.MadTransfer.Identities;
using MadWorldNL.MadTransfer.Web;

namespace MadWorldNL.MadTransfer.Builder;

internal static class RateLimiterExtensions
{
    internal static void AddDefaultRateLimiter(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, token) =>
            {
                const string retryAfterSeconds = "60";
        
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds;
        
                await context.HttpContext.Response.WriteAsync(
                    $"{{\"error\":\"Too Many Requests\",\"message\":\"Try again in {retryAfterSeconds} seconds.\"}}", token);
            };
    
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var ip = httpContext.GetIpAddress();

                return RateLimitPartition.GetTokenBucketLimiter(ip, _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 100,
                    QueueLimit = 0,
                    ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                    TokensPerPeriod = 100,
                    AutoReplenishment = true,
                });
            });
    
            options.AddPolicy(RateLimiterNames.PerUserPolicy, httpContext =>
            {
                var userId = httpContext.User.GetUserId();

                return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromSeconds(10),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });
        });
    }   
}