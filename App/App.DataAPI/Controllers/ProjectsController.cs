using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
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
    private readonly IProjectService _projectService;
    public ProjectsController(IValidator<UpdateProjectApiDto> updateValidator, IValidator<AddProjectApiDto> addValidator, IProjectService projectService)
    {
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _projectService = projectService;
    }

    [HttpGet("/all-projects")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _projectService.GetAllProjectsAsync();

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

            var result = await _projectService.AddProjectAsync(dto);

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


    
    [HttpDelete("/delete-project-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _projectService.DeleteProjectAsync(id);

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

            var result = await _projectService.UpdateProjectAsync(dto);

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

    [HttpGet("/get-project-{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _projectService.GetByIdAsync(id);

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

    [HttpGet("/change-project-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibilityAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _projectService.ChangeProjectVisibilityAsync(id);

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
