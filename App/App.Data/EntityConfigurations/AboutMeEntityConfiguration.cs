﻿using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.EntityConfigurations;
public class AboutMeEntityConfiguration : IEntityTypeConfiguration<AboutMeEntity>
{
    public void Configure(EntityTypeBuilder<AboutMeEntity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a=>a.Introduction).IsRequired();
        builder.Property(a => a.ImageUrl1).IsRequired().HasColumnType("varchar(255)");
        builder.Property(a => a.ImageUrl2).IsRequired().HasColumnType("varchar(255)");

    }
}