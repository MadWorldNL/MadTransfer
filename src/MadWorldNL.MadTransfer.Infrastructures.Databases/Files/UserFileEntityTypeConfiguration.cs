using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MadWorldNL.MadTransfer.Files;

public class UserFileEntityTypeConfiguration : IEntityTypeConfiguration<UserFile>
{
    public void Configure(EntityTypeBuilder<UserFile> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.Id)
            .HasConversion(
                fileId => fileId.Id,
                guid => new FileId(guid)
            );
        
        builder.Property(user => user.Url)
            .HasConversion(
                url => url.Value,
                value => Hyperlink.Create(value)
            );
        
        builder.Property(user => user.UserId)
            .HasConversion(
                userId => userId.Id,
                guid => new UserId(guid)
            );
    }
}