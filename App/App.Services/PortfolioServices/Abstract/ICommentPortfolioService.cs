using App.DTOs.CommentDtos.Portfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface ICommentPortfolioService
{
    Task<Result> AddCommentUnsignedAsync(AddCommentUnsignedDto dto);
    Task<Result> AddCommentSignedAsync(AddCommentSignedDto dto);
    Task<Result<List<BlogPostCommentsPortfolioDto>>> GetBlogPostCommentsAsync(int id);
    Task<Result> DeleteCommentAsync(int id);
}
