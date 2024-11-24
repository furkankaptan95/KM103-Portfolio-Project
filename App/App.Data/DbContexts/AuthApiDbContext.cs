﻿using App.Core.Helpers;
using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Data.DbContexts;
public class AuthApiDbContext : DbContext
{
    public  AuthApiDbContext(DbContextOptions<AuthApiDbContext> options) : base(options)
    {

    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<UserVerificationEntity> UserVerifications { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        byte[] passwordHash, passwordSalt;
        

        HashingHelper.CreatePasswordHash("123981.*.**", out passwordHash, out passwordSalt);

        modelBuilder.Entity<UserEntity>().HasData(

            new UserEntity
            {
                Id = 1,
                Username = "FurkanKaptan",
                Email = "iamfurkan86@gmail.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true,
                Role = "admin",

            },
            new UserEntity
            {
                Id = 2,
                Username = "FurkanKaptanC",
                Email = "Furkan.Kaptan.Work@gmail.com",
                IsActive = true,
                Role = "commenter",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            }
        );
    }
}
