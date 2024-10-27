using App.Data.DbContexts;
using App.DTOs.EducationDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class EducationPortfolioService(DataApiDbContext dataApiDb) : IEducationPortfolioService
{
    public async Task<Result<List<AllEducationsPortfolioDto>>> GetAllEducationsAsync()
    {
        try
        {
            var dtos = new List<AllEducationsPortfolioDto>();

            var entities = await dataApiDb.Educations.Where(bp => bp.IsVisible == true).ToListAsync();

            if (entities is null)
            {
                return Result<List<AllEducationsPortfolioDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllEducationsPortfolioDto
           {
               Degree = item.Degree,
               School = item.School,
               StartDate = item.StartDate,
               EndDate = item.EndDate,
           })
           .ToList();

            return Result<List<AllEducationsPortfolioDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllEducationsPortfolioDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllEducationsPortfolioDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
}
