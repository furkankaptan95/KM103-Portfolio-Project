using App.Core.Validators.DtoValidators.AuthValidators;
using App.DTOs.AuthDtos;
using App.Services.AuthService.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginDto> _loginValidator;
    public AuthController(IAuthService authService, IValidator<LoginDto> loginValidator)
    {
        _authService = authService;
        _loginValidator = loginValidator;
    }


    [HttpPost("/login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
    {
        try
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                if (result.Status == ResultStatus.Forbidden)
                {
                    return StatusCode(403, result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }

    }

    [HttpPost("/refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(Result<TokensDto>.Error("Token is null or empty"));
        }

        var result = await _authService.RefreshTokenAsync(token);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


    [HttpPost("/register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("/verify-email")]
    public async Task<IActionResult> VerifEmailAsync([FromBody] VerifyEmailDto dto)
    {
        var result = await _authService.VerifyEmailAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("/validate-token")]
    public async Task<IActionResult> ValidateTokenAsync([FromBody] string token)
    {
        var result = await _authService.ValidateTokenAsync(token);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("/forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDto dto)
    {
        var result = await _authService.ForgotPasswordAsync(dto);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost("/renew-password")]
    public async Task<IActionResult> RenewPasswordAsync([FromBody] RenewPasswordDto dto)
    {
        var result = await _authService.RenewPasswordEmailAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("/new-password")]
    public async Task<IActionResult> NewPasswordAsync([FromBody] NewPasswordDto dto)
    {
        var result = await _authService.NewPasswordAsync(dto);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost("/revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync([FromBody] string token)
    {
        var result = await _authService.RevokeTokenAsync(token);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
