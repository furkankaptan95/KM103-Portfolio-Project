using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.BlogPostDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.AdminServices;
public class BlogPostAdminService(DataApiDbContext dataApiDb) : IBlogPostAdminService
{
    public async Task<Result> AddBlogPostAsync(AddBlogPostDto dto)
    {
        try
        {
            var entity = new BlogPostEntity()
            {
                Title = dto.Title,
                Content = dto.Content,
                PublishDate = DateTime.Now,
            };

            await dataApiDb.BlogPosts.AddAsync(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result> ChangeBlogPostVisibilityAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.IsVisible = !entity.IsVisible;

            dataApiDb.BlogPosts.Update(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result> DeleteBlogPostAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            dataApiDb.BlogPosts.Remove(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result<List<AllBlogPostsAdminDto>>> GetAllBlogPostsAsync()
    {
        try
        {
            var dtos = new List<AllBlogPostsAdminDto>();

            var entities = await dataApiDb.BlogPosts.ToListAsync();

            if (entities is null)
            {
                return Result<List<AllBlogPostsAdminDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllBlogPostsAdminDto
           {
               Id = item.Id,
               Title = item.Title,
               Content = item.Content,
               PublishDate = item.PublishDate,
               IsVisible = item.IsVisible,
           })
           .ToList();

            return Result<List<AllBlogPostsAdminDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllBlogPostsAdminDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllBlogPostsAdminDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result<BlogPostToUpdateDto>> GetBlogPostById(int id)
    {
        try
        {
            var entity = await dataApiDb.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result<BlogPostToUpdateDto>.NotFound();
            }

            var dto = new BlogPostToUpdateDto
            {
                Id = id,
                Title = entity.Title,
                Content = entity.Content,
            };

            return Result<BlogPostToUpdateDto>.Success(dto);
        }
        catch (SqlException sqlEx)
        {
            return Result<BlogPostToUpdateDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<BlogPostToUpdateDto>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto)
    {
        try
        {
            var entity = await dataApiDb.BlogPosts.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.Title = dto.Title;
            entity.Content = dto.Content;
            entity.UpdatedAt = DateTime.Now;

            dataApiDb.BlogPosts.Update(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result.Error("Bir hata oluştu: " + ex.Message);
        }
    }
}
