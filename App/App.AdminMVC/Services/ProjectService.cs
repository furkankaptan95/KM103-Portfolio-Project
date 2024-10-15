using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class ProjectService(IHttpClientFactory factory) : IProjectService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> AddProjectAsync(AddProjectApiDto dto)
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

    public Task<Result> UpdateProjectAsync(UpdateProjectDto dto)
    {
        throw new NotImplementedException();
    }
}
