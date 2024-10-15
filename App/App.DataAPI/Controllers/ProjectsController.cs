using App.DTOs.ProjectDtos;
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

    public ProjectsController(IValidator<UpdateProjectApiDto> updateValidator, IValidator<AddProjectApiDto> addValidator)
    {
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }
}
