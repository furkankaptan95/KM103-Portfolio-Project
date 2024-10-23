using App.Core.Results;
using App.DTOs.AuthDtos;
using Ardalis.Result;

namespace App.Services;
public interface IAuthService
{
    Task<Result<TokensDto>> LoginAsync(LoginDto loginDto);
    Task<RegistrationResult> RegisterAsync(RegisterDto registerDto);
    Task<Result<TokensDto>> RefreshTokenAsync(string? token);
    Task<Result> RevokeTokenAsync(string token);
    Task<Result> VerifyEmailAsync(string email, string token);
    Task<Result> RenewPasswordEmailAsync(string email, string token);
    Task<Result> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<Result> ValidateTokenAsync(string token);
}
