using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EducationsController : ControllerBase
{
    private readonly IEducationService _educationService;
    public EducationsController(IEducationService educationService)
    {
        _educationService = educationService;
    }

    [HttpGet("/all-educations")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _educationService.GetAllEducationsAsync();

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }

    [HttpPost("/add-education")]
    public async Task<IActionResult> AddAsync([FromBody] AddEducationDto dto)
    {
        //validation eklenecek.

        var result = await _educationService.AddEducationAsync(dto);

        if (!result.IsSuccess)
        {
          return StatusCode(500, result);
        }

        return Ok(result);
    }
}
