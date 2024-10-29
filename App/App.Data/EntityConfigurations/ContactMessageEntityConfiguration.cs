using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class ContactMessageEntityConfiguration : IEntityTypeConfiguration<ContactMessageEntity>
{
    public void Configure(EntityTypeBuilder<ContactMessageEntity> builder)
    {
        builder.HasKey(cm => cm.Id);
        builder.Property(cm => cm.Id).ValueGeneratedOnAdd();

        builder.Property(cm=>cm.Name).IsRequired().HasColumnType("nvarchar(50)");
        builder.Property(cm => cm.Email).IsRequired().HasColumnType("nvarchar(100)");
        builder.Property(cm => cm.Subject).HasColumnType("nvarchar(100)");
        builder.Property(cm => cm.Message).IsRequired().HasColumnType("nvarchar(max)");
        builder.Property(c => c.SentDate).IsRequired().HasColumnType("datetime");
        builder.Property(c => c.IsRead).IsRequired().HasColumnType("bit");
        builder.Property(c => c.ReplyDate).HasColumnType("datetime");
    }
}
