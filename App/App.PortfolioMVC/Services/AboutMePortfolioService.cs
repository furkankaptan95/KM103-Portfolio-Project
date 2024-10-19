using App.DTOs.AboutMeDtos.Porfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.PortfolioMVC.Services;
public class AboutMePortfolioService(IHttpClientFactory factory) : IAboutMePortfolioService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result<AboutMePortfolioDto>> GetAboutMeAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("portfolio-get-about-me");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<AboutMePortfolioDto>>();

                if (result is null)
                {
                    return Result<AboutMePortfolioDto>.Error();
                }
                return result;
            }
            
            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
				return Result<AboutMePortfolioDto>.NotFound();
			}

            return Result<AboutMePortfolioDto>.Error();
        }
        catch (Exception)
        {
            return Result<AboutMePortfolioDto>.Error();
        }
    }
}
