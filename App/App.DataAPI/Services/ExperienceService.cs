using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.ExperienceDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
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

        public Task<Result<List<AllExperiencesDto>>> GetAllExperiencesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateExperienceAsync(UpdateExperienceDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
