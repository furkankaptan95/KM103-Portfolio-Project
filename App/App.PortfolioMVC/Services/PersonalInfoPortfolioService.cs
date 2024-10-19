using App.DTOs.PersonalInfoDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.PortfolioMVC.Services;
public class PersonalInfoPortfolioService(IHttpClientFactory factory) : IPersonalInfoPortfolioService
{
	private HttpClient DataApiClient => factory.CreateClient("dataApi");
	public async Task<Result<PersonalInfoPortfolioDto>> GetPersonalInfoAsync()
    {
		try
		{
			var apiResponse = await DataApiClient.GetAsync("portfolio-get-personal-info");

			if (apiResponse.IsSuccessStatusCode)
			{
				var result = await apiResponse.Content.ReadFromJsonAsync<Result<PersonalInfoPortfolioDto>>();
				if (result is null)
				{
					return Result<PersonalInfoPortfolioDto>.Error();
				}
				return result;
			}

			if (apiResponse.StatusCode == HttpStatusCode.NotFound)
			{
				return Result<PersonalInfoPortfolioDto>.NotFound();
			}

			return Result<PersonalInfoPortfolioDto>.Error();
		}

		catch (Exception)
		{
			return Result<PersonalInfoPortfolioDto>.Error();
		}
	}
}
