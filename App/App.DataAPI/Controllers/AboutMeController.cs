using App.DTOs.AboutMeDtos;
using App.DTOs.AboutMeDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutMeController : ControllerBase
{
    private readonly IAboutMeAdminService _aboutMeService;
    private readonly IValidator<AddAboutMeApiDto> _addValidator;
    private readonly IValidator<UpdateAboutMeApiDto> _updateValidator;
    public AboutMeController(IAboutMeAdminService aboutMeService, IValidator<AddAboutMeApiDto> addValidator, IValidator<UpdateAboutMeApiDto> updateValidator)
    {
        _aboutMeService = aboutMeService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet("/get-about-me")]
    public async Task<IActionResult> GetAboutMeAsync()
    {
        try
        {
            var result = await _aboutMeService.GetAboutMeAsync();

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound(result);
            }
            return StatusCode(500, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPost("/add-about-me")]
    public async Task<IActionResult> AddAboutMeAsync([FromBody] AddAboutMeApiDto dto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(dto);
            string errorMessage;

            if (!validationResult.IsValid)
            {
                errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _aboutMeService.AddAboutMeAsync(dto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return StatusCode(500, result);
        }
      
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [HttpPut("/update-about-me")]
    public async Task<IActionResult> UpdateAboutMeAsync([FromBody] UpdateAboutMeApiDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            string errorMessage;

            if (!validationResult.IsValid)
            {
                errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _aboutMeService.UpdateAboutMeAsync(dto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            if(result.Status == ResultStatus.NotFound)
            {
                return NotFound(result);
            }
            
            return StatusCode(500, result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }
}
