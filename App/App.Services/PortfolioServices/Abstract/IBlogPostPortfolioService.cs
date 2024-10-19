using App.DTOs.BlogPostDtos.Porfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IBlogPostPortfolioService
{
    Task<Result<List<HomeBlogPostsPortfolioDto>>> GetHomeBlogPostsAsync();
	Task<Result<SingleBlogPostDto>> GetBlogPostById(int id);
}
