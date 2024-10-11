using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class EducationService(IHttpClientFactory factory) : IEducationService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> AddEducationAsync(AddEducationDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ChangeEducationVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteEducationAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllEducationsDto>>> GetAllEducationsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateEducationAsync(UpdateEducationDto dto)
    {
        throw new NotImplementedException();
    }
}
