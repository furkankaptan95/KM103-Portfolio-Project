using App.Core.Results;
using App.DTOs.AuthDtos;
using Ardalis.Result;

namespace App.Services.AuthService.Abstract;
public interface IAuthService
{
    Task<Result<TokensDto>> LoginAsync(LoginDto loginDto);
    Task<RegistrationResult> RegisterAsync(RegisterDto registerDto);
    Task<Result<TokensDto>> RefreshTokenAsync(string token);
    Task<Result> RefreshTokenApiAsync(string token);
    Task<Result> RevokeTokenAsync(string token);
    Task<Result> VerifyEmailAsync(VerifyEmailDto dto);
    Task<Result> RenewPasswordEmailAsync(RenewPasswordDto dto);
    Task<Result> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<Result> ValidateTokenAsync(string token);
    Task<Result> NewPasswordAsync(NewPasswordDto dto);
    Task<Result> NewVerificationAsync(NewVerificationMailDto dto);
    
}
