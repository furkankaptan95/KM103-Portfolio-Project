using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services.PortfolioServices;
public class CommentPortfolioService : ICommentPortfolioService
{
    public Task<Result> AddCommentlAsync(AddCommentDto dto)
    {
        throw new NotImplementedException();
    }
}
