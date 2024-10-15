using App.DTOs.HomeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class HomeService(IHttpClientFactory factory) : IHomeService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result<HomeDto>> GetHomeInfosAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("get-home-infos");

        if (apiResponse.IsSuccessStatusCode)
        {
            return await apiResponse.Content.ReadFromJsonAsync<Result<HomeDto>>();
        }

        return Result<HomeDto>.Error();
    }
}
