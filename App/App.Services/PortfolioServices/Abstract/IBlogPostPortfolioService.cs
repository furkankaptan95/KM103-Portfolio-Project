using App.DTOs.BlogPostDtos.Porfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IBlogPostPortfolioService
{
    Task<Result<List<BlogPostsPortfolioDto>>> GetHomeBlogPostsAsync();
	Task<Result<SingleBlogPostDto>> GetBlogPostById(int id);
}
