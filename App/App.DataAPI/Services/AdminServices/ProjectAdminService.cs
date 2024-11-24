﻿using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.ProjectDtos;
using App.DTOs.ProjectDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.AdminServices;
public class ProjectAdminService(DataApiDbContext dataApiDb) : IProjectAdminService
{
    public async Task<Result> AddProjectAsync(AddProjectApiDto dto)
    {
        try
        {
            var entity = new ProjectEntity()
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
            };

            await dataApiDb.Projects.AddAsync(entity);
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
    public Task<Result> AddProjectAsync(AddProjectMVCDto dto)
    {
        throw new NotImplementedException();
    }
    public async Task<Result> ChangeProjectVisibilityAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.Projects.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.IsVisible = !entity.IsVisible;

            dataApiDb.Projects.Update(entity);
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
    public async Task<Result> DeleteProjectAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.Projects.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            dataApiDb.Projects.Remove(entity);
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
    public async Task<Result<List<AllProjectsAdminDto>>> GetAllProjectsAsync()
    {
        try
        {
            var dtos = new List<AllProjectsAdminDto>();

            var entities = await dataApiDb.Projects.ToListAsync();

            if (entities is null)
            {
                return Result<List<AllProjectsAdminDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllProjectsAdminDto
           {
               Id = item.Id,
               ImageUrl = item.ImageUrl,
               Description = item.Description,
               IsVisible = item.IsVisible,
               Title = item.Title,
           })
           .ToList();

            return Result<List<AllProjectsAdminDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllProjectsAdminDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllProjectsAdminDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
    public async Task<Result<ProjectToUpdateDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.Projects.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result<ProjectToUpdateDto>.NotFound();
            }

            var dto = new ProjectToUpdateDto
            {
                Id = id,
                ImageUrl = entity.ImageUrl,
                Title = entity.Title,
                Description = entity.Description,
            };

            return Result<ProjectToUpdateDto>.Success(dto);
        }
        catch (SqlException sqlEx)
        {
            return Result<ProjectToUpdateDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<ProjectToUpdateDto>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
    public Task<Result> UpdateProjectAsync(UpdateProjectMVCDto dto)
    {
        throw new NotImplementedException();
    }
    public async Task<Result> UpdateProjectAsync(UpdateProjectApiDto dto)
    {
        try
        {
            var entity = await dataApiDb.Projects.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null)
            {
                return Result.NotFound();
            }

            entity.Title = dto.Title;
            entity.Description = dto.Description;

            if (dto.ImageUrl != null)
            {
                entity.ImageUrl = dto.ImageUrl;
            }

            dataApiDb.Projects.Update(entity);
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
