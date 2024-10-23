using App.Core.Helpers;
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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        byte[] passwordHash, passwordSalt;
        

        HashingHelper.CreatePasswordHash("123Fk768", out passwordHash, out passwordSalt);

        modelBuilder.Entity<UserEntity>().HasData(

            new UserEntity
            {
                Id = 4,
                Username = "FurkanKaptan",
                Email = "iamfurkan86@gmail.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true,
                Role = "admin",

            },
             new UserEntity
             {
                 Id = 1,
                 Username = "hasansolmaz",
                 Email = "hslmz@gmail.com",
                 IsActive = true,
                 Role = "commenter",
                 PasswordHash = passwordHash,
                 PasswordSalt = passwordSalt,
             },
               new UserEntity
               {
                   Id = 2,
                   Username = "ardagüler",
                   Email = "ardglr@gmail.com",
                   IsActive = true,
                   Role = "commenter",
                   PasswordHash = passwordHash,
                   PasswordSalt = passwordSalt,
               },
               new UserEntity
               {
                   Id = 3,
                   Username = "fabrizioromano",
                   Email = "fromano@gmail.com",
                   IsActive = false,
                   Role = "commenter",
                   PasswordHash = passwordHash,
                   PasswordSalt = passwordSalt,
               }
            );
    }
}
