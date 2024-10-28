using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using App.DTOs.ExperienceDtos.Portfolio;
namespace App.PortfolioMVC.Services;
public class ExperiencePortfolioService(IHttpClientFactory factory) : IExperiencePortfolioService
{
	private HttpClient DataApiClient => factory.CreateClient("dataApi");
	public async Task<Result<List<AllExperiencesPortfolioDto>>> GetAllExperiencesAsync()
    {
		try
		{
			var apiResponse = await DataApiClient.GetAsync("portfolio-all-experiences");

			if (!apiResponse.IsSuccessStatusCode)
			{
				return Result<List<AllExperiencesPortfolioDto>>.Error();
			}

			var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllExperiencesPortfolioDto>>>();

			if (result is null)
			{
				return Result<List<AllExperiencesPortfolioDto>>.Error();
			}

			return result;
		}

		catch (Exception)
		{
			return Result<List<AllExperiencesPortfolioDto>>.Error();
		}
	}
}
