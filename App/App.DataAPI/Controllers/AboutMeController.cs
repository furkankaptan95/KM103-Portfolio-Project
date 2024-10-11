using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutMeController(IAboutMeService aboutMeService) : ControllerBase
{
    [HttpGet("/get-about-me")]
    public async Task<IActionResult> GetAboutMeAsync()
    {
        var result = await aboutMeService.GetAboutMeAsync();

        if (result == null || result.Value == null)
        {
            return NotFound();
        }

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return StatusCode(500, "An error occurred while processing your request.");
    }

    [HttpPost("/add-about-me")]
    public async Task<IActionResult> AddAboutMeAsync([FromBody] AddAboutMeDto dto)
    {
        var result = await aboutMeService.AddAboutMeAsync(dto);

        return Ok();
    }
}
