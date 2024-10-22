using App.DTOs.BlogPostDtos.Porfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.PortfolioMVC.Services;
public class BlogPostPortfolioService(IHttpClientFactory factory) : IBlogPostPortfolioService
{
	private HttpClient DataApiClient => factory.CreateClient("dataApi");
	public async Task<Result<List<HomeBlogPostsPortfolioDto>>> GetHomeBlogPostsAsync()
	{
        try
        {
            var apiResponse = await DataApiClient.GetAsync("home-blog-posts");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<HomeBlogPostsPortfolioDto>>>();

                if (result is null)
                {
                    return Result<List<HomeBlogPostsPortfolioDto>>.Error();
                }

                return result;
            }
            return Result<List<HomeBlogPostsPortfolioDto>>.Error();
        }

        catch (Exception)
        {
            return Result<List<HomeBlogPostsPortfolioDto>>.Error();
        }
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
