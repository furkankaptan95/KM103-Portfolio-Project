using App.DTOs.AuthDtos;
using Ardalis.Result;

namespace App.Services;
public interface IAuthService
{
    Task<Result<TokensDto>> LoginAsync(LoginDto loginDto);
}
