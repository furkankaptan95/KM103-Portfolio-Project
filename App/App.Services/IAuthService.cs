using App.Core.Results;
using App.DTOs.AuthDtos;
using Ardalis.Result;

namespace App.Services;
public interface IAuthService
{
    Task<Result<TokensDto>> LoginAsync(LoginDto loginDto);
    Task<RegistrationResult> RegisterAsync(RegisterDto registerDto);
    Task<Result<TokensDto>> RefreshTokenAsync(string? token);
}
