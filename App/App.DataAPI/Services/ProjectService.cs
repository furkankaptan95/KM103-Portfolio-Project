using App.Data.DbContexts;
using App.DTOs.ExperienceDtos;
using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class ProjectService(DataApiDbContext dataApiDb) : IProjectService
{
    public Task<Result> AddProjectAsync(AddProjectApiDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> AddProjectAsync(AddProjectMVCDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ChangeProjectVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteProjectAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllProjectsDto>>> GetAllProjectsAsync()
    {
        try
        {
            var dtos = new List<AllProjectsDto>();

            var entities = await dataApiDb.Projects.ToListAsync();

            if (entities is null)
            {
                return Result<List<AllProjectsDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllProjectsDto
           {
               Id = item.Id,
               ImageUrl = item.ImageUrl,
               Description = item.Description,
               IsVisible = item.IsVisible,
               Title = item.Title,
           })
           .ToList();

            return Result<List<AllProjectsDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllProjectsDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllProjectsDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public Task<Result<ProjectToUpdateDto>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateProjectAsync(UpdateProjectMVCDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateProjectAsync(UpdateProjectApiDto dto)
    {
        throw new NotImplementedException();
    }
}
