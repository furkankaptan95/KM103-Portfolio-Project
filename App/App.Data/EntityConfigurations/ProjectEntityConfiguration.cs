using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Title).IsRequired().HasColumnType("varchar(100)");
        builder.Property(p => p.Description).IsRequired();
        builder.Property(p => p.ImageUrl).IsRequired().HasColumnType("varchar(255)");
        builder.Property(e => e.IsVisible).IsRequired().HasColumnType("bit");
    }
}
