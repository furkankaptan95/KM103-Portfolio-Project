using App.DTOs.CommentDtos;
using App.DTOs.UserDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface ICommentAdminService
{
    Task<Result<List<AllCommentsDto>>> GetAllCommentsAsync();
    Task<Result> DeleteCommentAsync(int id);
    Task<Result> ApproveOrNotApproveCommentAsync(int id);
    Task<Result<List<UsersCommentsDto>>> GetUsersCommentsAsync(int id);
}
