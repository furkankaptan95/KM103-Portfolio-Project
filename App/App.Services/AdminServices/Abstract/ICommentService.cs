using App.DTOs.CommentDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface ICommentService
{
    Task<Result<List<AllCommentsDto>>> GetAllCommentsAsync();
    Task<Result> DeleteCommentAsync(int id);
    Task<Result> ApproveOrNotApproveCommentAsync(int id);
}
