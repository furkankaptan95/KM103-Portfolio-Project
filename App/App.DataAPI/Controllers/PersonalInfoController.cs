using App.DTOs.PersonalInfoDtos;
using App.DTOs.PersonalInfoDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonalInfoController : ControllerBase
{
    private readonly IPersonalInfoAdminService _personalInfoService;
    private readonly IValidator<AddPersonalInfoDto> _addValidator;
    private readonly IValidator<UpdatePersonalInfoDto> _updateValidator;
    public PersonalInfoController(IPersonalInfoAdminService personalInfoService, IValidator<AddPersonalInfoDto> addValidator, IValidator<UpdatePersonalInfoDto> updateValidator)
    {
        _personalInfoService = personalInfoService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }

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

            var result = await _personalInfoService.AddPersonalInfoAsync(dto);

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

    [HttpGet("/get-personal-info")]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var result = await _personalInfoService.GetPersonalInfoAsync();

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

            var result = await _personalInfoService.UpdatePersonalInfoAsync(dto);

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
