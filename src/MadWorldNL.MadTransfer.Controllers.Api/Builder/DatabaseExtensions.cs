using Microsoft.EntityFrameworkCore;

namespace MadWorldNL.MadTransfer.Builder;

internal static class DatabaseExtensions
{
    internal static void AddDefaultDatabase(this WebApplicationBuilder builder)
    {
        var databaseSettings = builder.Configuration
            .GetRequiredSection(DatabaseSettings.Key)
            .Get<DatabaseSettings>()!;

        builder.Services.AddDbContextPool<MadTransferContext>(opt =>
            opt.UseNpgsql(
                databaseSettings.ConnectionString,
                o => o
                    .SetPostgresVersion(17, 0)
                    .UseNodaTime()));
    }
    
    internal static void MigrateDatabase<TDbContext>(this IServiceProvider services) where TDbContext : DbContext
    {
        using var serviceScope = services.GetService<IServiceScopeFactory>()!.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        context.Database.Migrate();
    }
}