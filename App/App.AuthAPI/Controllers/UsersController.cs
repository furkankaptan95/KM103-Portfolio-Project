using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserAdminService _userAdminService;
    private readonly IUserPortfolioService _userPortfolioService;

    public UsersController(IUserAdminService userAdminService, IUserPortfolioService userPortfolioService)
    {
        _userAdminService = userAdminService;
        _userPortfolioService = userPortfolioService;
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

    [HttpPost("/edit-username")]
    public async Task<IActionResult> EditUsernameAsync([FromBody] EditUsernameDto dto)
    {
        try
        {
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

    [HttpPost("/edit-user-image")]
    public async Task<IActionResult> EditUserImageAsync([FromBody] EditUserImageApiDto dto)
    {
        try
        {
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

}
