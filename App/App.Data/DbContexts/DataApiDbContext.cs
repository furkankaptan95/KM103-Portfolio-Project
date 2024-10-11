using App.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Data.DbContexts;
public class DataApiDbContext : DbContext
{
    public DataApiDbContext(DbContextOptions<DataApiDbContext> options) : base(options)
    {

    }

    public DbSet<AboutMeEntity> AboutMes { get; set; }
    public DbSet<PersonalInfoEntity> PersonalInfos { get; set; }
    public DbSet<ExperienceEntity> Experiences { get; set; }
    public DbSet<EducationEntity> Educations { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ContactMessageEntity> ContactMessages { get; set; }
    public DbSet<BlogPostEntity> BlogPosts { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<AboutMeEntity>().HasData(

            new AboutMeEntity
            {
                Id = 1,
                ImageUrl1 = "default-img.jpg",
                ImageUrl2 = "default-img.jpg",
                Introduction = "Açıklama",

            }

            );
    }

}
