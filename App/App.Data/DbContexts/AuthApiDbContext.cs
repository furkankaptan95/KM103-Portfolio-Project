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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<UserEntity>().HasData(

            new UserEntity
            {
                Id = 1,
                Username = "FurkanKaptan",
                Email = "iamfurkan86@gmail.com",
                IsActive = true,
                ImageUrl = string.Empty,
                Role = "admin",

            }

            );
    }

}
