using App.DTOs.AuthDtos;
using App.DTOs.UserDtos;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IUserPortfolioService
{
    Task<Result<TokensDto>> EditUsernameAsync(EditUsernameDto dto);
    Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageMvcDto dto);
    Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageApiDto dto);
}
