using App.DTOs.CommentDtos.Admin;
using App.DTOs.UserDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface ICommentAdminService
{
    Task<Result<List<AllCommentsAdminDto>>> GetAllCommentsAsync();
    Task<Result> DeleteCommentAsync(int id);
    Task<Result> ApproveOrNotApproveCommentAsync(int id);
    Task<Result<List<UsersCommentsDto>>> GetUsersCommentsAsync(int id);
}
