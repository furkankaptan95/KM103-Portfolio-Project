﻿using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.ExperienceDtos;
using App.DTOs.ExperienceDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.AdminServices;
public class ExperienceAdminService(DataApiDbContext dataApiDb) : IExperienceAdminService
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
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
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
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
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
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
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
    public async Task<Result<List<AllExperiencesAdminDto>>> GetAllExperiencesAsync()
    {
        try
        {
            var dtos = new List<AllExperiencesAdminDto>();

            var entities = await dataApiDb.Experiences.ToListAsync();

            if (entities is null)
            {
                return Result<List<AllExperiencesAdminDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllExperiencesAdminDto
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

            return Result<List<AllExperiencesAdminDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllExperiencesAdminDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllExperiencesAdminDto>>.Error("Bir hata oluştu: " + ex.Message);
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

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.Company = dto.Company;
            entity.EndDate = dto.EndDate;
            entity.StartDate = dto.StartDate;
            entity.Title = dto.Title;
            entity.Description = dto.Description;

            dataApiDb.Experiences.Update(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
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
}
