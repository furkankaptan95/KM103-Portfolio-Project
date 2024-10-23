using App.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Data.EntityConfigurations;
public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.HasIndex(u => u.Token).IsUnique();

        builder.Property(c => c.ExpireDate).IsRequired().HasColumnType("datetime");
        builder.Property(c => c.IsRevoked).HasColumnType("datetime");
        builder.Property(c => c.IsUsed).HasColumnType("datetime");

        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired();
    }
}