namespace MadWorldNL.MadTransfer.Builder;

internal static class CorsExtensions
{
    internal static void AddDefaultCors(this WebApplicationBuilder builder, string allowedCors)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(allowedCors, policy =>
            {
                var origins = builder.Configuration
                    .GetRequiredSection("Cors:Origins")
                    .Get<string[]>()!;
        
                policy.WithOrigins(origins);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            });
        });
    }
}