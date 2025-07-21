using Microsoft.EntityFrameworkCore;
using MadWorldNL.MadTransfer.Files;

namespace MadWorldNL.MadTransfer;

public class MadTransferContext(DbContextOptions<MadTransferContext> options) : DbContext(options)
{
    public DbSet<UserFile> Files { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserFileEntityTypeConfiguration());
    }
}