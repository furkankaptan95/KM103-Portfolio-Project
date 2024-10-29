using App.Data.DbContexts;
using App.DTOs.ProjectDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class ProjectPortfolioService(DataApiDbContext dataApiDb) : IProjectPortfolioService
{
    public async Task<Result<List<AllProjectsPortfolioDto>>> GetAllProjectsAsync()
    {
        try
        {
            var dtos = new List<AllProjectsPortfolioDto>();

            var entities = await dataApiDb.Projects.Where(p => p.IsVisible == true).ToListAsync();

            if (entities is null)
            {
                return Result<List<AllProjectsPortfolioDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllProjectsPortfolioDto
           {
               ImageUrl = item.ImageUrl,
               Description = item.Description,
               Title = item.Title,
           })
           .ToList();

            return Result<List<AllProjectsPortfolioDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllProjectsPortfolioDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllProjectsPortfolioDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
}
