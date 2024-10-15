using App.DTOs.ExperienceDtos;
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
        var apiResponse = await DataApiClient.GetAsync("all-projects");

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result<List<AllProjectsDto>>.Error("Projeler getirilirken beklenmedik bir hata oluştu..");
        }

        return await apiResponse.Content.ReadFromJsonAsync<Result<List<AllProjectsDto>>>();
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
