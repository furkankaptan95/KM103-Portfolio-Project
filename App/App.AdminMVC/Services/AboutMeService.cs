using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class AboutMeService(IHttpClientFactory factory) : IAboutMeService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    private HttpClient FileApiClient => factory.CreateClient("fileApi");
    public async Task<Result> AddAboutMeAsync(AddAboutMeMVCDto dto)
    {
        var response = await DataApiClient.PostAsJsonAsync("add-about-me", dto);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public Task<Result> AddAboutMeAsync(AddAboutMeApiDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ShowAboutMeDto>> GetAboutMeAsync()
    {
        var response = await DataApiClient.GetAsync("get-about-me");


        if (response.IsSuccessStatusCode)
        {
            // JSON verisini doğrudan Result<ShowAboutMeDto> olarak deseralize et
            var result = await response.Content.ReadFromJsonAsync<Result<ShowAboutMeDto>>();

            return result;
        }

        // Eğer yanıt başarısızsa, duruma göre NotFound ya da hata dönebilirsiniz
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return Result<ShowAboutMeDto>.NotFound();
        }

        // Diğer hata durumları için uygun bir sonuç döndür
        return Result<ShowAboutMeDto>.Error("An error occurred while fetching the data.");
    }

    public Task<Result> UpdateAboutMeAsync(UpdateAboutMeDto dto)
    {
        throw new NotImplementedException();
    }
}
