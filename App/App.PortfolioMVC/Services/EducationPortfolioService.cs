using App.DTOs.EducationDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class EducationPortfolioService(IHttpClientFactory factory) : IEducationPortfolioService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result<List<AllEducationsPortfolioDto>>> GetAllEducationsAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("portfolio-all-educations");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllEducationsPortfolioDto>>.Error();
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllEducationsPortfolioDto>>>();

            if (result is null)
            {
                return Result<List<AllEducationsPortfolioDto>>.Error();
            }

            return result;
        }

        catch (Exception)
        {
            return Result<List<AllEducationsPortfolioDto>>.Error();
        }
    }
}
