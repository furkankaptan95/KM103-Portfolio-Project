using App.DTOs.ProjectDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class ProjectPortfolioService(IHttpClientFactory factory) : IProjectPortfolioService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result<List<AllProjectsPortfolioDto>>> GetAllProjectsAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("all-projects");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllProjectsPortfolioDto>>.Error();
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllProjectsPortfolioDto>>>();

            if (result is null)
            {
                return Result<List<AllProjectsPortfolioDto>>.Error();
            }

            return result;
        }

        catch (Exception)
        {
            return Result<List<AllProjectsPortfolioDto>>.Error();
        }
    }
}
