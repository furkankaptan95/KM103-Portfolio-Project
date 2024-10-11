using App.DTOs.HomeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class HomeService(IHttpClientFactory factory) : IHomeService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result<HomeDto>> GetHomeInfosAsync()
    {
        throw new NotImplementedException();
    }
}
