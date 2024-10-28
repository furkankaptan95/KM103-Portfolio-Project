using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class BlogPostEntityConfiguration : IEntityTypeConfiguration<BlogPostEntity>
{
    public void Configure(EntityTypeBuilder<BlogPostEntity> builder)
    {
        builder.HasKey(bp => bp.Id);
        builder.Property(bp => bp.Id).ValueGeneratedOnAdd();

        builder.Property(bp=>bp.Title).IsRequired().HasColumnType("nvarchar(100)");
        builder.Property(bp => bp.Content).IsRequired().HasColumnType("nvarchar(max)");
        builder.Property(bp => bp.PublishDate).IsRequired().HasColumnType("datetime");
        builder.Property(bp => bp.UpdatedAt).HasColumnType("datetime");
        builder.Property(bp=>bp.IsVisible).IsRequired().HasColumnType("bit");

        builder.HasMany(bp => bp.Comments)
            .WithOne(c => c.BlogPost)
            .HasForeignKey(c => c.BlogPostId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
