using App.Core.Validators.EducationValidators;
using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EducationsController : ControllerBase
{
    private readonly IEducationService _educationService;
    private readonly IValidator<AddEducationDto> _addValidator;
    public EducationsController(IEducationService educationService, IValidator<AddEducationDto> addValidator)
    {
        _educationService = educationService;
        _addValidator = addValidator;
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
        var validationResult = await _addValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
        }

        var result = await _educationService.AddEducationAsync(dto);

        if (!result.IsSuccess)
        {
          return StatusCode(500, result);
        }

        return Ok(result);
    }
}
