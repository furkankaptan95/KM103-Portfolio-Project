using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class CvEntityConfiguration : IEntityTypeConfiguration<CvEntity>
{
    public void Configure(EntityTypeBuilder<CvEntity> builder)
    {

        builder.HasKey(cm => cm.Id);
        builder.Property(cm => cm.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.Url).IsRequired().HasColumnType("nvarchar(255)");
    }
}
