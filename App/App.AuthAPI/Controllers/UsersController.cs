using App.Data.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(AuthApiDbContext authApiDbContext) : ControllerBase
{
    [HttpGet("/get-users-count")]
    public async Task<IActionResult> GetCount()
    {
        return Ok(await authApiDbContext.Users.CountAsync());
    }
}
