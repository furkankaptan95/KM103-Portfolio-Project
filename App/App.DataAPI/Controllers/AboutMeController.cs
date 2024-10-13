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
    private readonly IValidator<AddAboutMeApiDto> _addValidator;
    private readonly IValidator<UpdateAboutMeApiDto> _updateValidator;
    public AboutMeController(IAboutMeService aboutMeService, IValidator<AddAboutMeApiDto> addValidator, IValidator<UpdateAboutMeApiDto> updateValidator)
    {
        _aboutMeService = aboutMeService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }


    [HttpGet("/get-about-me")]
    public async Task<IActionResult> GetAboutMeAsync()
    {
        var result = await _aboutMeService.GetAboutMeAsync();

        if (result.IsSuccess)
        {
            // Eğer başarı durumu varsa, DTO'yu geri döndür
            return Ok(result.Value); // Success durumunda DTO'yu döndürüyoruz
        }

        else if (result.Status == ResultStatus.NotFound)
        {
            return NotFound();
        }

        else
        {
            return StatusCode(500, result.Errors.FirstOrDefault()); // Genel hata varsa hata mesajını döndür
        }
    }

    [HttpPost("/add-about-me")]
    public async Task<IActionResult> AddAboutMeAsync([FromBody] AddAboutMeApiDto dto)
    {
        var validationResult = await _addValidator.ValidateAsync(dto);
        string errorMessage;

        if (!validationResult.IsValid)
        {
            errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return BadRequest(Result.Error(errorMessage));
        }

        var result = await _aboutMeService.AddAboutMeAsync(dto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        errorMessage = result.Errors.FirstOrDefault();

        return StatusCode(500,errorMessage);
    }


    [HttpPost("/update-about-me")]
    public async Task<IActionResult> UpdateAboutMeAsync([FromBody] UpdateAboutMeApiDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        string errorMessage;

        if(!validationResult.IsValid)
        {
            errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return BadRequest(Result.Error(errorMessage));
        }

        var result = await _aboutMeService.UpdateAboutMeAsync(dto);

        if (result.IsSuccess)
        {
            return Ok();
        }

        errorMessage = result.Errors.FirstOrDefault();

        return StatusCode(500, errorMessage);
    }
}
