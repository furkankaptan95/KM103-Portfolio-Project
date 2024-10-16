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

        builder.Property(u => u.Username).IsRequired().HasColumnType("varchar(50)");
        builder.Property(u => u.Email).IsRequired().HasColumnType("varchar(100)");
        //builder.Property(u => u.PasswordHash).IsRequired().HasColumnType("varchar(255)");
        //builder.Property(u => u.PasswordSalt).IsRequired().HasColumnType("varchar(255)");
        builder.Property(u => u.Role).IsRequired().HasColumnType("varchar(50)");
        builder.Property(u => u.ImageUrl).HasColumnType("varchar(255)");
        builder.Property(u => u.IsActive).HasColumnType("bit");

    }
}
