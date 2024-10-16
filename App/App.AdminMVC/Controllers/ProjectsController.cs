using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.ProjectsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ProjectsController(IProjectService projectService) : Controller
{
    [HttpGet]
    [Route("all-projects")]
    public async Task<IActionResult> AllProjects()
    {
        var result = await projectService.GetAllProjectsAsync();

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/home/index");
        }

        var models = new List<AllProjectsViewModel>();
        var dtos = result.Value;

            models = dtos
           .Select(item => new AllProjectsViewModel
           {
               Id = item.Id,
               Title = item.Title,
               ImageUrl= item.ImageUrl,
               Description = item.Description,
               IsVisible = item.IsVisible
           })
           .ToList();

        return View(models);
    }

    [HttpGet]
    [Route("add-project")]
    public async Task<IActionResult> AddProject()
    {
        return View();
    }

    [HttpPost]
    [Route("add-project")]
    public async Task<IActionResult> AddProject([FromForm] AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new AddProjectMVCDto
        {
            ImageFile = model.ImageFile,
            Title = model.Title,
            Description = model.Description,
        };

        var result = await projectService.AddProjectAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-projects");
    }

    [HttpGet]
    [Route("update-project-{id:int}")]
    public async Task<IActionResult> UpdateProject([FromRoute] int id)
    {
        var result = await projectService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/all-projects");
        }

        var dto = result.Value;

        var model = new UpdateProjectViewModel
        {
            Id = id,
            ImageUrl = dto.ImageUrl,
            Title = dto.Title,
            Description = dto.Description,
        };

        return View(model);
    }

    [HttpPost]
    [Route("update-project")]
    public async Task<IActionResult> UpdateProject([FromForm] UpdateProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new UpdateProjectMVCDto
        {
            Title = model.Title,
            Description = model.Description,
            Id = model.Id,
            ImageFile = model.ImageFile,
        };

        var result = await projectService.UpdateProjectAsync(dto);

        if (!result.IsSuccess)
        {
            if (result.Status == ResultStatus.NotFound)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/all-projects");
            }

            ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return View();
        }

        TempData["Message"] = result.SuccessMessage;

        return Redirect("/all-projects");
    }

    [HttpGet]
    [Route("delete-project-{id:int}")]
    public async Task<IActionResult> DeleteProject([FromRoute] int id)
    {
        var result = await projectService.DeleteProjectAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-projects");
    }


    [HttpGet]
    [Route("change-project-visibility-{id:int}")]
    public async Task<IActionResult> ChangeProjectVisibility([FromRoute] int id)
    {
        var result = await projectService.ChangeProjectVisibilityAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-projects");
    }

}
