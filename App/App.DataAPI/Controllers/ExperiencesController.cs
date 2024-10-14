using App.DTOs.EducationDtos;
using App.DTOs.ExperienceDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExperiencesController : ControllerBase
{
    private readonly IExperienceService _experiencesService;
    private readonly IValidator<AddExperienceDto> _addValidator;
    private readonly IValidator<UpdateExperienceDto> _updateValidator;
    public ExperiencesController(IExperienceService experiencesService, IValidator<AddExperienceDto> addValidator, IValidator<UpdateExperienceDto> updateValidator)
    {
        _experiencesService = experiencesService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost("/add-experience")]
    public async Task<IActionResult> AddAsync([FromBody] AddExperienceDto dto)
    {
        var validationResult = await _addValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
        }

        var result = await _experiencesService.AddExperienceAsync(dto);

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }

    [HttpGet("/all-experiences")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _experiencesService.GetAllExperiencesAsync();

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }

    [HttpPut("/update-experience")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateExperienceDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
        }

        var result = await _experiencesService.UpdateExperienceAsync(dto);

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }

    [HttpGet("/get-experience-{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var result = await _experiencesService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound(result);
            }

            return StatusCode(500, result);
        }

        return Ok(result);
    }

    [HttpDelete("/delete-experience-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {

        var result = await _experiencesService.DeleteExperienceAsync(id);

        if (!result.IsSuccess)
        {
            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound(result);
            }

            return StatusCode(500, result);
        }

        return Ok(result);
    }


    [HttpGet("/change-experience-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibilityAsync([FromRoute] int id)
    {
        var result = await _experiencesService.ChangeExperienceVisibilityAsync(id);

        if (!result.IsSuccess)
        {
            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound(result);
            }
            return StatusCode(500, result);
        }
        return Ok(result);
    }

}
