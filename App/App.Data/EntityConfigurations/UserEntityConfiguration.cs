using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.Username).IsRequired().HasColumnType("nvarchar(50)");
        builder.Property(u => u.Email).IsRequired().HasColumnType("nvarchar(100)");
        builder.Property(u => u.PasswordHash).IsRequired().HasColumnType("nvarchar(255)");
        builder.Property(u => u.PasswordSalt).IsRequired().HasColumnType("nvarchar(255)");
        builder.Property(u => u.Role).IsRequired().HasColumnType("nvarchar(50)");
        builder.Property(u => u.ImageUrl).HasColumnType("nvarchar(255)");
        builder.Property(u => u.IsActive).IsRequired().HasColumnType("bit");

    }
}
