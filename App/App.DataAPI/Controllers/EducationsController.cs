using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EducationsController : ControllerBase
{
    [HttpGet("/get-all-educations")]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }
}
