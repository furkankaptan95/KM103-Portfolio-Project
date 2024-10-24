using App.DTOs.AuthDtos;
using App.Services.AuthService.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("/login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
    {
        var result = await authService.LoginAsync(dto);

        if (!result.IsSuccess)
        {
            if(result.Status == ResultStatus.NotFound)
            {
                return NotFound(result);
            }

            if(result.Status == ResultStatus.Forbidden)
            {
                return StatusCode(403, result);
            }

            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("/refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(Result<TokensDto>.Error("Token is null or empty"));
        }

        var result = await authService.RefreshTokenAsync(token);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


    [HttpPost("/register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
    {
        var result = await authService.RegisterAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("/verify-email")]
    public async Task<IActionResult> VerifEmailAsync([FromBody] VerifyEmailDto dto)
    {
        var result = await authService.VerifyEmailAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("/validate-token")]
    public async Task<IActionResult> ValidateTokenAsync([FromBody] string token)
    {
        var result = await authService.ValidateTokenAsync(token);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("/forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDto dto)
    {
        var result = await authService.ForgotPasswordAsync(dto);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost("/renew-password")]
    public async Task<IActionResult> RenewPasswordAsync([FromBody] RenewPasswordDto dto)
    {
        var result = await authService.RenewPasswordEmailAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
