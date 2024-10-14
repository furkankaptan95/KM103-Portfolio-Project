using App.Services.AdminServices.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EducationsController : ControllerBase
{
    private readonly IEducationService _educationService;
    public EducationsController(IBlogPostService educationService)
    {
        _educationService = _educationService;
    }

    [HttpGet("/all-educations")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _educationService.GetAllEducationsAsync();

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }
}
