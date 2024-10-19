using App.DTOs.BlogPostDtos.Porfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.PortfolioMVC.Services;
public class BlogPosPortfolioService(IHttpClientFactory factory) : IBlogPosPortfolioService
{
	private HttpClient DataApiClient => factory.CreateClient("dataApi");
	public Task<Result<List<HomeBlogPostsPortfolioDto>>> GetHomeBlogPostsAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Result<SingleBlogPostDto>> GetBlogPostById(int id)
	{
		try
		{
			var apiResponse = await DataApiClient.GetAsync($"portfolio-blog-post-{id}");

			if (apiResponse.IsSuccessStatusCode)
			{
				var result = await apiResponse.Content.ReadFromJsonAsync<Result<SingleBlogPostDto>>();

				if (result is null)
				{
					return Result<SingleBlogPostDto>.Error();
				}

				return result;
			}

			string errorMessage;

			if (apiResponse.StatusCode == HttpStatusCode.NotFound)
			{
				return Result<SingleBlogPostDto>.NotFound();
			}

			return Result<SingleBlogPostDto>.Error();
		}

		catch (Exception)
		{
			return Result<SingleBlogPostDto>.Error();
		}
	}
}
