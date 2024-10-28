using App.Core.Results;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserAdminService _userAdminService;
    private readonly IUserPortfolioService _userPortfolioService;
    private readonly IValidator<EditUsernameDto> _editUsernameValidator; 
    private readonly IValidator<EditUserImageApiDto> _editUserImageValidator;

    public UsersController(IUserAdminService userAdminService, IUserPortfolioService userPortfolioService, IValidator<EditUsernameDto> editUsernameValidator, IValidator<EditUserImageApiDto> editUserImageValidator)
    {
        _userAdminService = userAdminService;
        _userPortfolioService = userPortfolioService;
        _editUsernameValidator = editUsernameValidator;
        _editUserImageValidator = editUserImageValidator;
    }

    [HttpGet("/get-users-count")]
    public async Task<IActionResult> GetCountAsync()
    {
        try
        {
            var result = await _userAdminService.GetUsersCount();

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpGet("/get-commenter-username-{id:int}")]
    public async Task<IActionResult> GetCommentsUserNameAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _userAdminService.GetCommentsUserName(id);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpGet("/all-users")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _userAdminService.GetAllUsersAsync();

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpGet("/change-user-activeness-{id:int}")]
    public async Task<IActionResult> ChangeActivenessOfUserAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _userAdminService.ChangeActivenessOfUserAsync(id);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPut("/edit-username")]
    public async Task<IActionResult> EditUsernameAsync([FromBody] EditUsernameDto dto)
    {
        try
        {
            var validationResult = await _editUsernameValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _userPortfolioService.EditUsernameAsync(dto);

            if (!result.IsSuccess)
            {
                if(result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                if(result.Status == ResultStatus.Unavailable)
                {
                    return BadRequest(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPut("/change-user-image")]
    public async Task<IActionResult> EditUserImageAsync([FromBody] EditUserImageApiDto dto)
    {
        try
        {
            var validationResult = await _editUserImageValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _userPortfolioService.ChangeUserImageAsync(dto);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpDelete("/delete-user-img/{imgUrl}")]
    public async Task<IActionResult> DeleteUserImageAsync([FromRoute] string imgUrl)
    {
        if (!string.IsNullOrEmpty(imgUrl))
        {
            return BadRequest(Result.Invalid());
        }

        try
        {
            var result = await _userPortfolioService.DeleteUserImageAsync(imgUrl);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

}
