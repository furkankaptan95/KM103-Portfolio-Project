using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class CommentPortfolioService : ICommentPortfolioService
{
    public Task<Result> AddCommentSignedAsync(AddCommentSignedDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> AddCommentUnsignedAsync(AddCommentUnsignedDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<BlogPostCommentsPortfolioDto>>> GetBlogPostCommentsAsync(int id)
	{
		throw new NotImplementedException();
	}
}
