using App.Data.Entities;
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

        modelBuilder.Entity<BlogPostEntity>().HasData(
              new BlogPostEntity
              {
                  Id = 1,
                  Title = "Sınırsız Ülke",
                  Content = "üşünmeden, belki de haklı olarak birine zarar verdikten sonra Talia, Kolombiya’daki ergen kızlara yönelik bir ıslahevine gönderilir. Ancak bir an önce oradan kaçıp Amerika uçağına yetişmek zorundadır. Eğer o uçuşu kaçırırsa, on iki yıldır ayrı kaldığı annesini ve kardeşlerini bir daha göremeyecektir.\r\nKendisi de bir göçmen olan Patricia Engel, bu romanda sınırların insanda yarattığı tahribatı, onuru ayaklar altına alan göçmen politikalarının bir aileyi nasıl darmaduman edebildiğini, sevginin şiddet karşısında nasıl çaresiz kalabileceğini anlatıyor. Hikâye boyunca Kolombiya’nın kültürel zenginlikleri ve And Dağları eteklerinde dolaşan mitler de bize eşlik ediyor.",
                  IsVisible = true,
                  PublishDate = DateTime.Now.AddDays(-100),
              }, new BlogPostEntity
              {
                  Id = 2,
                  Title = "Ne Değişir Ki?",
                  Content = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                  IsVisible = true,
                  PublishDate = DateTime.Now.AddDays(-200),
              }
              );

        modelBuilder.Entity<CommentEntity>().HasData(
            new CommentEntity
            {
                Content = "Güzel bir yazı olmuş.",
                CreatedAt = DateTime.Now,
                Id = 1,
                IsApproved = true,
                BlogPostId = 1,
                UserId = 1,
            }, 
            new CommentEntity
            {
                Content = "Harika!",
                CreatedAt = DateTime.Now.AddMinutes(-10),
                Id = 2,
                IsApproved = false,
                BlogPostId = 1,
                UnsignedCommenterName = "Yakışıklı Çocuk",
            },
             new CommentEntity
             {
                 Content = "Beğenmedim pek.",
                 CreatedAt = DateTime.Now.AddDays(-35),
                 Id = 3,
                 IsApproved = false,
                 BlogPostId = 2,
                 UserId = 2,
             },
            new CommentEntity
            {
                Content = "Eh işte!",
                CreatedAt = DateTime.Now.AddDays(-3),
                Id = 4,
                IsApproved = true,
                BlogPostId = 2,
                UnsignedCommenterName = "Güzel İnsan",
            }
            );
    }
}
