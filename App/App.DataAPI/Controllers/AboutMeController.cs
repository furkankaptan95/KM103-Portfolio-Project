using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutMeController : ControllerBase
{
    [HttpGet("/get-about-me")]
    public async Task<IActionResult> GetAboutMeAsync()
    {
        return Ok();
    }
}
