using App.DTOs.AuthDtos;
using App.DTOs.UserDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IUserAdminService
{
    Task<Result<List<AllUsersDto>>> GetAllUsersAsync();
    Task<Result> ChangeActivenessOfUserAsync(int id);
    Task<Result<string>> GetCommentsUserName(int id);
    Task<Result<int>> GetUsersCount();
    Task<Result<TokensDto>> EditUsernameAsync(EditUsernameDto dto);
    Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageMvcDto dto);
    Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageApiDto dto);
    Task<Result<TokensDto>> DeleteUserImageAsync(string imgUrl);
}
