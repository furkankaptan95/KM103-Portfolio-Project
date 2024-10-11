using App.DTOs.ExperienceDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class ExperienceService(IHttpClientFactory factory) : IExperienceService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> AddExperienceAsync(AddExperienceDto dto)
    {
        throw new NotImplementedException();
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
