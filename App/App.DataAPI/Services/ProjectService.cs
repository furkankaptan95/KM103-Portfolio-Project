using App.Data.DbContexts;
using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

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

    public Task<Result<List<AllProjectsDto>>> GetAllProjectsAsync()
    {
        throw new NotImplementedException();
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
