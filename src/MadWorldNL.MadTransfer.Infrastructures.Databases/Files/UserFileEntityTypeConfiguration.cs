using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MadWorldNL.MadTransfer.Files;

public class UserFileEntityTypeConfiguration : IEntityTypeConfiguration<UserFile>
{
    public void Configure(EntityTypeBuilder<UserFile> builder)
    {
        builder.HasKey(file => file.Id);
        
        builder.Property(file => file.Id)
            .HasConversion(
                fileId => fileId.Id,
                guid => new FileId(guid)
            );
        
        builder.OwnsOne(file => file.MetaData, meta =>
        {
            meta.Property(m => m.Name).HasColumnName("FileName");
            meta.Property(m => m.InternalName).HasColumnName("FileInternalName");
            meta.Property(m => m.Extension).HasColumnName("FileExtension");
            meta.Property(m => m.ByteSize).HasColumnName("FileByteSize");
        });
        
        builder.Property(user => user.Url)
            .HasConversion(
                url => url.Value,
                value => Hyperlink.FromDatabase(value)
            );
        
        builder.Property(user => user.UserId)
            .HasConversion(
                userId => userId.Id,
                guid => new UserId(guid)
            );
    }
}