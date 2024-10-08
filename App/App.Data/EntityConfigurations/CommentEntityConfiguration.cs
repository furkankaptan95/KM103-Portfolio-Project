using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c=>c.Content).IsRequired();
        builder.Property(c=>c.CreatedAt).IsRequired().HasColumnType("datetime");
        builder.Property(c => c.IsApproved).HasColumnType("bit");
        builder.Property(c => c.UnsignedCommenterName).HasMaxLength(50);

        builder.HasOne(c => c.BlogPost)
            .WithMany(bp => bp.Comments)
            .HasForeignKey(c => c.BlogPostId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c=>c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c=>c.UserId)
            .OnDelete(DeleteBehavior.NoAction);      
    }
}
