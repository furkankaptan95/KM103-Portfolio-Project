using App.Core.Authorization;
using App.DTOs.PersonalInfoDtos;
using App.DTOs.PersonalInfoDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonalInfoController : ControllerBase
{
    private readonly IPersonalInfoAdminService _personalInfoAdminService;
	private readonly IPersonalInfoPortfolioService _personalInfoPortfolioService;
	private readonly IValidator<AddPersonalInfoDto> _addValidator;
    private readonly IValidator<UpdatePersonalInfoDto> _updateValidator;
    public PersonalInfoController(IPersonalInfoAdminService personalInfoAdminService, IValidator<AddPersonalInfoDto> addValidator, IValidator<UpdatePersonalInfoDto> updateValidator,IPersonalInfoPortfolioService personalInfoPortfolioService)
    {
		_personalInfoAdminService = personalInfoAdminService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _personalInfoPortfolioService = personalInfoPortfolioService;
    }

    [AuthorizeRolesApi("admin")]
    [HttpGet("/check-personal-info")]
    public async Task<IActionResult> CheckPersonalInfoAsync()
    {
        try
        {
            var result = await _personalInfoAdminService.CheckPersonalInfoAsync();

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

    [AuthorizeRolesApi("admin")]
    [HttpPost("/add-personal-info")]
    public async Task<IActionResult> AddAsync([FromBody] AddPersonalInfoDto dto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _personalInfoAdminService.AddPersonalInfoAsync(dto);

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
    [AuthorizeRolesApi("admin")]
    [HttpGet("/get-personal-info")]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var result = await _personalInfoAdminService.GetPersonalInfoAsync();

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
    [AllowAnonymousManuel]
    [HttpGet("/portfolio-get-personal-info")]
	public async Task<IActionResult> GetPortfolioAsync()
	{
		try
		{
			var result = await _personalInfoPortfolioService.GetPersonalInfoAsync();

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
    [AuthorizeRolesApi("admin")]
    [HttpPut("/update-personal-info")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdatePersonalInfoDto dto)
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

            var result = await _personalInfoAdminService.UpdatePersonalInfoAsync(dto);

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
}
