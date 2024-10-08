using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Data.DbContexts;
public class AuthApiDbContext : DbContext
{
    public  AuthApiDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

}
