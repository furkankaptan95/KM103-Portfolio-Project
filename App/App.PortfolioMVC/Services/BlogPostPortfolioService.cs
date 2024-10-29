using App.DTOs.BlogPostDtos.Porfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.PortfolioMVC.Services;
public class BlogPostPortfolioService(IHttpClientFactory factory) : IBlogPostPortfolioService
{
	private HttpClient DataApiClient => factory.CreateClient("dataApi");
	public async Task<Result<List<BlogPostsPortfolioDto>>> GetHomeBlogPostsAsync()
	{
        try
        {
            var apiResponse = await DataApiClient.GetAsync("home-blog-posts");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<BlogPostsPortfolioDto>>>();

                if (result is null)
                {
                    return Result<List<BlogPostsPortfolioDto>>.Error();
                }

                return result;
            }

            return Result<List<BlogPostsPortfolioDto>>.Error();
        }

        catch (Exception)
        {
            return Result<List<BlogPostsPortfolioDto>>.Error();
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
