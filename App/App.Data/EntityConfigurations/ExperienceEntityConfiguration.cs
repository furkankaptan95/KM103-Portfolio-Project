using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class ExperienceEntityConfiguration : IEntityTypeConfiguration<ExperienceEntity>
{
    public void Configure(EntityTypeBuilder<ExperienceEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e=>e.Title).IsRequired().HasColumnType("varchar(100)");
        builder.Property(e => e.Company).IsRequired().HasColumnType("varchar(100)");
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.StartDate).IsRequired().HasColumnType("datetime");
        builder.Property(e => e.EndDate).HasColumnType("datetime");
        builder.Property(e => e.IsVisible).IsRequired().HasColumnType("bit");
    }
}
