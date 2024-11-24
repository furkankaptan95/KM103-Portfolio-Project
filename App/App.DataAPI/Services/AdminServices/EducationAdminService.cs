﻿using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.EducationDtos;
using App.DTOs.EducationDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.AdminServices;
public class EducationAdminService(DataApiDbContext dataApiDb) : IEducationAdminService
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
    public async Task<Result> ChangeEducationVisibilityAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.Educations.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.IsVisible = !entity.IsVisible;

            dataApiDb.Educations.Update(entity);
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
    public async Task<Result> DeleteEducationAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.Educations.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            dataApiDb.Educations.Remove(entity);
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
    public async Task<Result<List<AllEducationsAdminDto>>> GetAllEducationsAsync()
    {
        try
        {
            var dtos = new List<AllEducationsAdminDto>();

            var entities = await dataApiDb.Educations.ToListAsync();

            if (entities is null)
            {
                return Result<List<AllEducationsAdminDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllEducationsAdminDto
           {
               Id = item.Id,
               Degree = item.Degree,
               School = item.School,
               StartDate = item.StartDate,
               EndDate = item.EndDate,
               IsVisible = item.IsVisible,
           })
           .ToList();

            return Result<List<AllEducationsAdminDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllEducationsAdminDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllEducationsAdminDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
    public async Task<Result<EducationToUpdateDto>> GetEducationByIdAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.Educations.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result<EducationToUpdateDto>.NotFound();
            }

            var dto = new EducationToUpdateDto
            {
                Id = id,
                School = entity.School,
                Degree = entity.Degree,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
            };

            return Result<EducationToUpdateDto>.Success(dto);
        }
        catch (SqlException sqlEx)
        {
            return Result<EducationToUpdateDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<EducationToUpdateDto>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
    public async Task<Result> UpdateEducationAsync(UpdateEducationDto dto)
    {
        try
        {
            var entity = await dataApiDb.Educations.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.School = dto.School;
            entity.EndDate = dto.EndDate;
            entity.StartDate = dto.StartDate;
            entity.Degree = dto.Degree;

            dataApiDb.Educations.Update(entity);
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
