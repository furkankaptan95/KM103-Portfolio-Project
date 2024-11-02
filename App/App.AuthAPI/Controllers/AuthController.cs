using App.Core.Authorization;
using App.Core.Results;
using App.DTOs.AuthDtos;
using App.Services.AuthService.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymousManuel]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<ForgotPasswordDto> _forgotPasswordValidator;
    private readonly IValidator<RenewPasswordDto> _renewPasswordValidator;
    private readonly IValidator<NewPasswordDto> _newPasswordValidator;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<VerifyEmailDto> _verifyEmailValidator;
    private readonly IValidator<NewVerificationMailDto> _newEmailValidator;
    public AuthController(IAuthService authService, IValidator<LoginDto> loginValidator, IValidator<ForgotPasswordDto> forgotPasswordValidator, IValidator<RenewPasswordDto> renewPasswordValidator, IValidator<NewPasswordDto> newPasswordValidator, IValidator<RegisterDto> registerValidator, IValidator<VerifyEmailDto> verifyEmailValidator, IValidator<NewVerificationMailDto> newEmailValidator)
    {
        _authService = authService;
        _loginValidator = loginValidator;
        _forgotPasswordValidator = forgotPasswordValidator;
        _renewPasswordValidator = renewPasswordValidator;
        _newPasswordValidator = newPasswordValidator;
        _registerValidator = registerValidator;
        _verifyEmailValidator = verifyEmailValidator;
        _newEmailValidator = newEmailValidator;
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
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(Result<TokensDto>.Invalid());
            }

            var result = await _authService.RefreshTokenAsync(token);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPost("/register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
    {
        try
        {
            var validationResult = await _registerValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new RegistrationResult(false,errorMessage,Core.Enums.RegistrationError.None));
            }

            var result = await _authService.RegisterAsync(dto);

            if (!result.IsSuccess)
            {
                if(result.Error == Core.Enums.RegistrationError.None)
                {
                    return StatusCode(500,result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }
      
         catch (Exception ex)
        {
            return StatusCode(500, new RegistrationResult(false,$"Bir hata oluştu: {ex.Message}",Core.Enums.RegistrationError.None));
        }
    }

    [HttpPost("/verify-email")]
    public async Task<IActionResult> VerifEmailAsync([FromBody] VerifyEmailDto dto)
    {
        try
        {
            var validationResult = await _verifyEmailValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new RegistrationResult(false, errorMessage, Core.Enums.RegistrationError.None));
            }

            var result = await _authService.VerifyEmailAsync(dto);

            if (!result.IsSuccess)
            {
                if(result.Status == ResultStatus.Invalid)
                {
                    return BadRequest(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPost("/validate-token")]
    public async Task<IActionResult> ValidateTokenAsync([FromBody] string token)
    {
        try
        {
            var result = await _authService.ValidateTokenAsync(token);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPost("/forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDto dto)
    {
        try
        {
            var validationResult = await _forgotPasswordValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _authService.ForgotPasswordAsync(dto);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPost("/new-verification")]
    public async Task<IActionResult> NewVerificationAsync([FromBody] NewVerificationMailDto dto)
    {
        try
        {
            var validationResult = await _newEmailValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _authService.NewVerificationAsync(dto);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }
    }


    [HttpPost("/renew-password")]
    public async Task<IActionResult> RenewPasswordAsync([FromBody] RenewPasswordDto dto)
    {
        try
        {
            var validationResult = await _renewPasswordValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _authService.RenewPasswordEmailAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPost("/new-password")]
    public async Task<IActionResult> NewPasswordAsync([FromBody] NewPasswordDto dto)
    {
        try
        {
            var validationResult = await _newPasswordValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _authService.NewPasswordAsync(dto);

            if (!result.IsSuccess)
            {
                if(result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }

          catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPost("/revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync([FromBody] string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(Result.Invalid());
        }

        try
        {
            var result = await _authService.RevokeTokenAsync(token);

            if (!result.IsSuccess)
            {
                if(result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Bir hata oluştu: {ex.Message}"));
        }
    }
}
