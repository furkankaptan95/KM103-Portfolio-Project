using App.Core;
using App.DTOs.ProjectDtos;
using App.DTOs.ProjectDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IValidator<UpdateProjectApiDto> _updateValidator;
    private readonly IValidator<AddProjectApiDto> _addValidator;
    private readonly IProjectAdminService _projectAdminService;
    private readonly IProjectPortfolioService _projectPortfolioService;
    public ProjectsController(IValidator<UpdateProjectApiDto> updateValidator, IValidator<AddProjectApiDto> addValidator, IProjectAdminService projectAdminService,IProjectPortfolioService projectPortfolioService)
    {
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _projectAdminService = projectAdminService;
        _projectPortfolioService = projectPortfolioService;
    }
    [AuthorizeRoles("admin")]
    [HttpGet("/all-projects")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _projectAdminService.GetAllProjectsAsync();

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluþtu: {ex.Message}"));
        }
    }
    [AllowAnonymousManuel]
    [HttpGet("/porfolio-all-projects")]
    public async Task<IActionResult> GetAllPortfolioAsync()
    {
        try
        {
            var result = await _projectPortfolioService.GetAllProjectsAsync();

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluþtu: {ex.Message}"));
        }
    }
    [AuthorizeRoles("admin")]
    [HttpPost("/add-project")]
    public async Task<IActionResult> AddAsync([FromBody] AddProjectApiDto dto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _projectAdminService.AddProjectAsync(dto);

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluþtu: {ex.Message}"));
        }
    }


    [AuthorizeRoles("admin")]
    [HttpDelete("/delete-project-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _projectAdminService.DeleteProjectAsync(id);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluþtu: {ex.Message}"));
        }
    }
    [AuthorizeRoles("admin")]
    [HttpPut("/update-project")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateProjectApiDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _projectAdminService.UpdateProjectAsync(dto);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluþtu: {ex.Message}"));
        }
    }
    [AuthorizeRoles("admin")]
    [HttpGet("/get-project-{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _projectAdminService.GetByIdAsync(id);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluþtu: {ex.Message}"));
        }
    }
    [AuthorizeRoles("admin")]
    [HttpGet("/change-project-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibilityAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _projectAdminService.ChangeProjectVisibilityAsync(id);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluþtu: {ex.Message}"));
        }
    }
}
