using App.Data.DbContexts;
using App.DTOs.HomeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class HomeService(DataApiDbContext dataApiDb,AuthApiDbContext authApiDb) : IHomeService
{
    public async Task<Result<HomeDto>> GetHomeInfosAsync()
    {
        try
        {
            var dto = new HomeDto();

            dto.CommentsCount = await dataApiDb.Comments.CountAsync();
            dto.ProjectsCount = await dataApiDb.Projects.CountAsync();
            dto.BlogPostsCount = await dataApiDb.BlogPosts.CountAsync();
            dto.ExperiencesCount = await dataApiDb.Experiences.CountAsync();
            dto.EducationsCount = await dataApiDb.Educations.CountAsync();
            dto.UsersCount = await authApiDb.Users.CountAsync();

            return Result<HomeDto>.Success(dto);
        }
        catch (SqlException sqlEx)
        {
            return Result<HomeDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<HomeDto>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
}
