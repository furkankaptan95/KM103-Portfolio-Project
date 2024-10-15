using App.Data.DbContexts;
using App.DTOs.HomeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class HomeService : IHomeService
{
    private readonly DataApiDbContext _dataApiDb;
    private readonly IHttpClientFactory _factory;
    public HomeService(DataApiDbContext dataApiDb, IHttpClientFactory factory)
    {
        _dataApiDb = dataApiDb;
        _factory = factory;
    }
    private HttpClient AuthApiClient => _factory.CreateClient("authApi");
    public async Task<Result<HomeDto>> GetHomeInfosAsync()
    {
        try
        {

            var apiResponse = await AuthApiClient.GetAsync("get-users-count");

            var dto = new HomeDto();

            dto.CommentsCount = await _dataApiDb.Comments.CountAsync();
            dto.ProjectsCount = await _dataApiDb.Projects.CountAsync();
            dto.BlogPostsCount = await _dataApiDb.BlogPosts.CountAsync();
            dto.ExperiencesCount = await _dataApiDb.Experiences.CountAsync();
            dto.EducationsCount = await _dataApiDb.Educations.CountAsync();
            dto.UsersCount = await apiResponse.Content.ReadFromJsonAsync<int>();

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
