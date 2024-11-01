using App.Core.Authorization;
using App.DTOs.ExperienceDtos;
using App.DTOs.ExperienceDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ExperiencesController : ControllerBase
{
    private readonly IExperienceAdminService _experiencesAdminService;
	private readonly IExperiencePortfolioService _experiencesPortfolioService;
	private readonly IValidator<AddExperienceDto> _addValidator;
    private readonly IValidator<UpdateExperienceDto> _updateValidator;
    public ExperiencesController(IExperienceAdminService experiencesAdminService, IValidator<AddExperienceDto> addValidator, IValidator<UpdateExperienceDto> updateValidator, IExperiencePortfolioService experiencesPortfolioService)
    {
        _experiencesAdminService = experiencesAdminService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _experiencesPortfolioService = experiencesPortfolioService;
    }

    [AuthorizeRolesApi("admin")]
    [HttpPost("/add-experience")]
    public async Task<IActionResult> AddAsync([FromBody] AddExperienceDto dto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _experiencesAdminService.AddExperienceAsync(dto);

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [AuthorizeRolesApi("admin")]
    [HttpGet("/all-experiences")]
    public async Task<IActionResult> GetAllAsync()
    {
        try 
        {
            var result = await _experiencesAdminService.GetAllExperiencesAsync();

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [AllowAnonymousManuel]
	[HttpGet("/portfolio-all-experiences")]
	public async Task<IActionResult> GetAllPortfolioAsync()
	{
		try
		{
			var result = await _experiencesPortfolioService.GetAllExperiencesAsync();

			if (!result.IsSuccess)
			{
				return StatusCode(500, result);
			}

			return Ok(result);
		}

		catch (Exception ex)
		{
			return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
		}
	}

    [AuthorizeRolesApi("admin")]
    [HttpPost("/update-experience")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateExperienceDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _experiencesAdminService.UpdateExperienceAsync(dto);

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

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [AuthorizeRolesApi("admin")]
    [HttpGet("/get-experience-{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _experiencesAdminService.GetByIdAsync(id);

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

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [AuthorizeRolesApi("admin")]
    [HttpGet("/delete-experience-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _experiencesAdminService.DeleteExperienceAsync(id);

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

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }

    [AuthorizeRolesApi("admin")]
    [HttpGet("/change-experience-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibilityAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _experiencesAdminService.ChangeExperienceVisibilityAsync(id);

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
        
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }
}
