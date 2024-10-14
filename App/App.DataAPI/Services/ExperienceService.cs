using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.EducationDtos;
using App.DTOs.ExperienceDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services
{
    public class ExperienceService(DataApiDbContext dataApiDb) : IExperienceService
    {
        public async Task<Result> AddExperienceAsync(AddExperienceDto dto)
        {
            try
            {
                var entity = new ExperienceEntity()
                {
                    Title = dto.Title,
                    Company = dto.Company,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Description = dto.Description,
                };

                await dataApiDb.Experiences.AddAsync(entity);
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

        public Task<Result> ChangeExperienceVisibilityAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteExperienceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<AllExperiencesDto>>> GetAllExperiencesAsync()
        {
            try
            {
                var dtos = new List<AllExperiencesDto>();

                var entities = await dataApiDb.Experiences.ToListAsync();

                if (entities is null)
                {
                    return Result<List<AllExperiencesDto>>.Success(dtos);
                }

                dtos = entities
               .Select(item => new AllExperiencesDto
               {
                   Id = item.Id,
                   Company = item.Company,
                   Description = item.Description,
                   StartDate = item.StartDate,
                   EndDate = item.EndDate,
                   IsVisible = item.IsVisible,
                   Title = item.Title,
               })
               .ToList();

                return Result<List<AllExperiencesDto>>.Success(dtos);
            }
            catch (SqlException sqlEx)
            {
                return Result<List<AllExperiencesDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                return Result<List<AllExperiencesDto>>.Error("Bir hata oluştu: " + ex.Message);
            }
        }

        public Task<Result> UpdateExperienceAsync(UpdateExperienceDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
