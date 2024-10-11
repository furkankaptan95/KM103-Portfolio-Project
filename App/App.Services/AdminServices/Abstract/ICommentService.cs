using App.DTOs.CommentDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface ICommentService
{
    Task<Result<List<AllCommentsDto>>> GetAllComments();
    Task<Result> DeleteComment(int id);
    Task<Result> ApproveOrNotApproveComment(int id);
}
