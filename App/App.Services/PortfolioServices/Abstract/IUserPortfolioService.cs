using App.DTOs.UserDtos;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IUserPortfolioService
{
    Task<Result> EditUsernameAsync(EditUsernameDto dto);
}
