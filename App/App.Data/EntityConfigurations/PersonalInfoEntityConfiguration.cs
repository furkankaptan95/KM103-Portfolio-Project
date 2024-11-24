﻿using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class PersonalInfoEntityConfiguration : IEntityTypeConfiguration<PersonalInfoEntity>
{
    public void Configure(EntityTypeBuilder<PersonalInfoEntity> builder)
    {
        builder.HasKey(pi => pi.Id);
        builder.Property(pi => pi.Id).ValueGeneratedOnAdd();

        builder.Property(pi=>pi.About).IsRequired().HasColumnType("nvarchar(300)");
        builder.Property(pi => pi.Name).IsRequired().HasColumnType("nvarchar(50)");
        builder.Property(pi => pi.Email).IsRequired().HasColumnType("nvarchar(100)");
        builder.Property(pi => pi.Adress).IsRequired().HasColumnType("nvarchar(50)");
        builder.Property(pi => pi.Link).IsRequired().HasColumnType("nvarchar(255)");
        builder.Property(pi => pi.Surname).IsRequired().HasColumnType("nvarchar(50)");
        builder.Property(c => c.BirthDate).IsRequired().HasColumnType("datetime");
    }
}
