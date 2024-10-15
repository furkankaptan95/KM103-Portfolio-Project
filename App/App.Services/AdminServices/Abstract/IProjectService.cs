using App.DTOs.ProjectDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IProjectService
{
    Task<Result<List<AllProjectsDto>>> GetAllProjectsAsync();
    Task<Result> AddProjectAsync(AddProjectApiDto dto);
    Task<Result> UpdateProjectAsync(UpdateProjectDto dto);
    Task<Result> DeleteProjectAsync(int id);
    Task<Result> ChangeProjectVisibilityAsync(int id);
}
