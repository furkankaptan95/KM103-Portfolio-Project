using App.Data.DbContexts;
using App.DTOs.BlogPostDtos;
using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class EducationService(DataApiDbContext dataApiDb) : IEducationService
{
    public Task<Result> AddEducationAsync(AddEducationDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ChangeEducationVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteEducationAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllEducationsDto>>> GetAllEducationsAsync()
    {
        try
        {
            var dtos = new List<AllEducationsDto>();

            var entities = await dataApiDb.Educations.ToListAsync();

            if (entities is null)
            {
                return Result.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllEducationsDto
           {
               Id = item.Id,
               Degree = item.Degree,
               School = item.School,
               StartDate = item.StartDate,
               EndDate = item.EndDate,
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

    public Task<Result> UpdateEducationAsync(UpdateEducationDto dto)
    {
        throw new NotImplementedException();
    }
}
