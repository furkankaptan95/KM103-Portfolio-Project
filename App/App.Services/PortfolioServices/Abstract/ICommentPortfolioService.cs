using App.DTOs.CommentDtos.Portfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface ICommentPortfolioService
{
    Task<Result> AddCommentlAsync(AddCommentDto dto);
}
