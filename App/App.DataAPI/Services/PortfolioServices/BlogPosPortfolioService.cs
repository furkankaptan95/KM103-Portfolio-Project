using App.DTOs.BlogPostDtos.Porfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services.PortfolioServices;
public class BlogPosPortfolioService : IBlogPosPortfolioService
{
	public Task<Result<List<AllBlogPostsPortfolioDto>>> GetAllBlogPostsAsync()
	{
		throw new NotImplementedException();
	}

	public Task<Result<SingleBlogPostDto>> GetBlogPostById(int id)
	{
		throw new NotImplementedException();
	}
}
