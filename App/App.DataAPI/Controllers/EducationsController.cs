﻿using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

public class EducationsController : ControllerBase
{
    private readonly IEducationAdminService _educationAdminService;
    private readonly IEducationPortfolioService _educationPortfolioService;
    private readonly IValidator<AddEducationDto> _addValidator;
    private readonly IValidator<UpdateEducationDto> _updateValidator;
    public EducationsController(IEducationAdminService educationAdminService, IValidator<AddEducationDto> addValidator, IValidator<UpdateEducationDto> updateValidator, IEducationPortfolioService educationPortfolioService)
    {
        _educationAdminService = educationAdminService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _educationPortfolioService = educationPortfolioService;
    }
    [Authorize(Roles = "admin")]
    [HttpGet("/all-educations")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _educationAdminService.GetAllEducationsAsync();

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
    [AllowAnonymous]
    [HttpGet("/portfolio-all-educations")]
    public async Task<IActionResult> GetAllPortfolioAsync()
    {
        try
        {
            var result = await _educationPortfolioService.GetAllEducationsAsync();

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
    [Authorize(Roles = "admin")]
    [HttpPost("/add-education")]
    public async Task<IActionResult> AddAsync([FromBody] AddEducationDto dto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _educationAdminService.AddEducationAsync(dto);

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

    [Authorize(Roles = "admin")]
    [HttpPost("/update-education")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateEducationDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _educationAdminService.UpdateEducationAsync(dto);

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
    [Authorize(Roles = "admin")]
    [HttpGet("/get-education-{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _educationAdminService.GetEducationByIdAsync(id);

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
    [Authorize(Roles = "admin")]
    [HttpGet("/delete-education-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _educationAdminService.DeleteEducationAsync(id);

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
    [Authorize(Roles = "admin")]
    [HttpGet("/change-education-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibilityAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _educationAdminService.ChangeEducationVisibilityAsync(id);

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
