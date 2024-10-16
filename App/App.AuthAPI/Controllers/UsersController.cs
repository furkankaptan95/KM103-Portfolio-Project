using App.Services.AdminServices.Abstract;
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
        return Ok();
    }

    [HttpGet("/get-all-users")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        return Ok();
    }

    [HttpGet("/change-user-activeness-{id:int}")]
    public async Task<IActionResult> ChangeActivenessOfUserAsync([FromRoute] int id)
    {
        return Ok();
    }
}
