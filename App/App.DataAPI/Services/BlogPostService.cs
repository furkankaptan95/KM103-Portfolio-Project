﻿using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.BlogPostsViewModels;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class BlogPostService(DataApiDbContext dataApiDb) : IBlogPostService
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

            return Result.SuccessWithMessage(" Yeni BlogPost başarıyla eklendi. ");
        }
        catch (DbUpdateException dbEx)
        {
            return Result.Error("Veritabanı hatası: " + dbEx.Message);
        }
        catch (Exception ex)
        {
            return Result.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public Task<Result> ChangeBlogPostVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteBlogPostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllBlogPostsDto>>> GetAllBlogPostsAsync()
    {
        try
        {
            var dtos = new List<AllBlogPostsDto>();

            var entities = await dataApiDb.BlogPosts.ToListAsync();
            
            if(entities is null)
            {
                return Result.Success(dtos);
            }

             dtos = entities
            .Select(item => new AllBlogPostsDto
            {
               Id = item.Id,
               Title = item.Title,
               Content = item.Content,
               PublishDate = item.PublishDate,
               IsVisible = item.IsVisible,
            })
            .ToList();

            return Result.Success(dtos);
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

    public Task<Result<BlogPostToUpdateDto>> GetBlogPostById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto)
    {
        throw new NotImplementedException();
    }
}