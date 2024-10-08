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
    }

}
