using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class CommentPortfolioService : ICommentPortfolioService
{
    public Task<Result> AddCommentAsync(AddCommentDto dto)
    {
        throw new NotImplementedException();
    }

	public Task<Result<List<BlogPostCommentsPortfolioDto>>> GetBlogPostCommentsAsync(int id)
	{
		throw new NotImplementedException();
	}
}
