using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutMeController : ControllerBase
{
    private readonly IAboutMeService _aboutMeService;
    private readonly IValidator<AddAboutMeApiDto> _validator;
    public AboutMeController(IAboutMeService aboutMeService, IValidator<AddAboutMeApiDto> validator)
    {
        _aboutMeService = aboutMeService;
        _validator = validator;
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
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return BadRequest(Result.Error(errorMessage));
        }

        var result = await _aboutMeService.AddAboutMeAsync(dto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return StatusCode(500, "An error occurred while processing your request.");
    }
}
