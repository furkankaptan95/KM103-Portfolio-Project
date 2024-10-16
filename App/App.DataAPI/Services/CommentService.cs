using App.Data.DbContexts;
using App.DTOs.CommentDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services;
public class CommentService(DataApiDbContext dataApiDb) : ICommentService
{
    public Task<Result> ApproveOrNotApproveCommentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteCommentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllCommentsDto>>> GetAllCommentsAsync()
    {
        throw new NotImplementedException();
    }
}
