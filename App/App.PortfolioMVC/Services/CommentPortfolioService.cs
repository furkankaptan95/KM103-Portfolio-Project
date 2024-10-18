using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class CommentPortfolioService : ICommentPortfolioService
{
    public Task<Result> AddCommentlAsync(AddCommentDto dto)
    {
        throw new NotImplementedException();
    }
}
