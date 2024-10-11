using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class AboutMeService(IHttpClientFactory factory) : IAboutMeService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> AddAboutMeAsync(AddAboutMeDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ShowAboutMeDto>> GetAboutMeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAboutMeAsync(UpdateAboutMeDto dto)
    {
        throw new NotImplementedException();
    }
}
