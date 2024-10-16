using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("/get-users-count")]
    public async Task<IActionResult> GetCountAsync()
    {
        return Ok();
    }

    [HttpGet("/get-commenter-username-{id:int}")]
    public async Task<IActionResult> GetCommentsUserNameAsync([FromRoute] int id)
    {
        var result = await _userService.GetCommentsUserName(id);

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

    [HttpGet("/all-users")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _userService.GetAllUsersAsync();

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }

    [HttpGet("/change-user-activeness-{id:int}")]
    public async Task<IActionResult> ChangeActivenessOfUserAsync([FromRoute] int id)
    {
        return Ok();
    }
}
