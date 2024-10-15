using App.DTOs.AboutMeDtos;
using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonalInfoController : ControllerBase
{
    private readonly IPersonalInfoService _personalInfoService;
    private readonly IValidator<AddPersonalInfoDto> _addValidator;
    public PersonalInfoController(IPersonalInfoService personalInfoService, IValidator<AddPersonalInfoDto> addValidator)
    {
        _personalInfoService = personalInfoService;
        _addValidator = addValidator;
    }

    [HttpPost("/add-personal-info")]
    public async Task<IActionResult> AddAsync([FromBody] AddPersonalInfoDto dto)
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

    [HttpGet("/get-personal-info")]
    public async Task<IActionResult> GetAsync()
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

    [HttpPut("/update-personal-info")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdatePersonalInfoDto dto)
    {
        //var validationResult = await _updateValidator.ValidateAsync(dto);
        //string errorMessage;

        //if (!validationResult.IsValid)
        //{
        //    errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

        //    return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
        //}

        var result = await _personalInfoService.UpdatePersonalInfoAsync(dto);

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
}
