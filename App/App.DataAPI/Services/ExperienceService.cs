using App.Data.DbContexts;
using App.Data.Entities;
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

        public async Task<Result> ChangeExperienceVisibilityAsync(int id)
        {
            try
            {
                var entity = await dataApiDb.Experiences.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is null)
                {
                    return Result.NotFound();
                }

                entity.IsVisible = !entity.IsVisible;

                dataApiDb.Experiences.Update(entity);
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

        public async Task<Result> DeleteExperienceAsync(int id)
        {
            try
            {
                var entity = await dataApiDb.Experiences.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is null)
                {
                    return Result.NotFound();
                }

                dataApiDb.Experiences.Remove(entity);
                await dataApiDb.SaveChangesAsync();

                return Result.Success();
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

        public async Task<Result<ExperienceToUpdateDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await dataApiDb.Experiences.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is null)
                {
                    return Result<ExperienceToUpdateDto>.NotFound();
                }

                var dto = new ExperienceToUpdateDto
                {
                    Id = id,
                    Company = entity.Company,
                    Title = entity.Title,
                    Description = entity.Description,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                };

                return Result<ExperienceToUpdateDto>.Success(dto);
            }
            catch (SqlException sqlEx)
            {
                return Result<ExperienceToUpdateDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                return Result<ExperienceToUpdateDto>.Error("Bir hata oluştu: " + ex.Message);
            }
        }

        public async Task<Result> UpdateExperienceAsync(UpdateExperienceDto dto)
        {
            try
            {
                var entity = await dataApiDb.Experiences.FirstOrDefaultAsync(x => x.Id == dto.Id);

                entity.Company = dto.Company;
                entity.EndDate = dto.EndDate;
                entity.StartDate = dto.StartDate;
                entity.Title = dto.Title;
                entity.Description = dto.Description;

                dataApiDb.Experiences.Update(entity);
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
}
