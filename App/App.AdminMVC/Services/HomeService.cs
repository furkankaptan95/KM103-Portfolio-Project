using App.DTOs.HomeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class HomeService(IHttpClientFactory factory) : IHomeService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result<HomeDto>> GetHomeInfosAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("get-home-infos");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<HomeDto>>();

                if (result is null)
                {
                    return Result<HomeDto>.Error();
                }

                return result;
            }

            return Result<HomeDto>.Error();
        }
       
          catch (Exception)
        {
            return Result<HomeDto>.Error();
        }
    }
}
