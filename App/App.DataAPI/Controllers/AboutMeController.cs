using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutMeController : ControllerBase
{
    private readonly IAboutMeService _aboutMeService;
    public AboutMeController(IAboutMeService aboutMeService)
    {
        _aboutMeService = aboutMeService;
    }


    [HttpGet("/get-about-me")]
    public async Task<IActionResult> GetAboutMeAsync()
    {
        var result = await _aboutMeService.GetAboutMeAsync();

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
    public async Task<IActionResult> AddAboutMeAsync([FromBody] AddAboutMeApiDto dto)
    {


        var result = await _aboutMeService.AddAboutMeAsync(dto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return StatusCode(500, "An error occurred while processing your request.");
    }
}
