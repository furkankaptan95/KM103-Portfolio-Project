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

    [HttpGet("/get-all-educations")]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }
}
