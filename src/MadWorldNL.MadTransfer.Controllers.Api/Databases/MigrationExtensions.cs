using Microsoft.EntityFrameworkCore;

namespace MadWorldNL.MadTransfer.Databases;

internal static class MigrationExtensions
{
    internal static void MigrateDatabase<TDbContext>(this IServiceProvider services) where TDbContext : DbContext
    {
        using var serviceScope = services.GetService<IServiceScopeFactory>()!.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        context.Database.Migrate();
    }
}