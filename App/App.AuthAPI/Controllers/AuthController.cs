using App.DTOs.AuthDtos;
using App.Services;
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
}
