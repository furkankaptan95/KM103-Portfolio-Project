using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using FluentValidation;
using Microsoft.AspNetCore.Http;
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

    [HttpGet("/all-experiences")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _projectService.GetAllProjectsAsync();

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }
}
