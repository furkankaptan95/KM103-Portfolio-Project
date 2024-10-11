using App.DTOs.CommentDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class CommentService : ICommentService
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
