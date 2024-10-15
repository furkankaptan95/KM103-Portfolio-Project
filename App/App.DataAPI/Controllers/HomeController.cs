using App.Services.AdminServices.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IHomeService homeService) : ControllerBase
{
    [HttpGet("/get-home-infos")]
    public async Task<IActionResult> GetHomeInfosAsync()
    {
        var result = await homeService.GetHomeInfosAsync();

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }
}
