using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.BlogPostDtos;
using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class EducationService(DataApiDbContext dataApiDb) : IEducationService
{
    public async Task<Result> AddEducationAsync(AddEducationDto dto)
    {
        try
        {
            var entity = new EducationEntity()
            {
               Degree = dto.Degree,
               StartDate = dto.StartDate,
               EndDate = dto.EndDate,
               School = dto.School,
            };

            await dataApiDb.Educations.AddAsync(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
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

    public Task<Result<EducationToUpdateDto>> GetEducationByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateEducationAsync(UpdateEducationDto dto)
    {
        try
        {
            var entity = await dataApiDb.Educations.FirstOrDefaultAsync(x => x.Id == dto.Id);

            entity.School = dto.School;
            entity.EndDate = dto.EndDate;
            entity.StartDate = dto.StartDate;
            entity.Degree = dto.Degree;

            dataApiDb.Educations.Update(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
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
}
