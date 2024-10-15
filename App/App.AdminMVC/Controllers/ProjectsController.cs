using App.Data.Entities;
using App.DTOs.BlogPostDtos;
using App.DTOs.PersonalInfoDtos;
using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.ExperiencesViewModels;
using App.ViewModels.AdminMvc.ProjectsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ProjectsController(IProjectService projectService) : Controller
{
    private static int index = 0;
    private static readonly List<ProjectEntity> _projects = new List<ProjectEntity>
    {
        new()
        {
            Id = ++index,
            Title = "proje 1",
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur at justo nec risus malesuada condimentum id condimentum ipsum. Nulla ut erat nec magna ultricies pulvinar ut nec neque. Curabitur nunc dui, ullamcorper sed sodales et, faucibus eget neque. Aenean tincidunt, elit ac fermentum laoreet.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur at justo nec risus malesuada condimentum id condimentum ipsum. Nulla ut erat nec magna ultricies pulvinar ut nec neque. Curabitur nunc dui, ullamcorper sed sodales et, faucibus eget neque. Aenean tincidunt, elit ac fermentum laoreet.",
            ImageUrl = "default-img.jpg"
        },
          new()
        {
            Id = ++index,
            Title = "proje 2",
            Description = "2. Proje Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur at justo nec risus malesuada condimentum id condimentum ipsum. Nulla ut erat nec magna ultricies pulvinar ut nec neque. Curabitur nunc dui, ullamcorper sed sodales et, faucibus eget neque. Aenean tincidunt, elit ac fermentum laoreet.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur at justo nec risus malesuada condimentum id condimentum ipsum. Nulla ut erat nec magna ultricies pulvinar ut nec neque. Curabitur nunc dui, ullamcorper sed sodales et, faucibus eget neque. Aenean tincidunt, elit ac fermentum laoreet.",
            ImageUrl = "default-img.jpg"
        },
    };

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

        if (dtos.Count > 0)
        {
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
        }

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
    public async Task<IActionResult> AddProject([FromForm] AddProjectViewModel addProjectModel)
    {
        if (!ModelState.IsValid)
        {
            return View(addProjectModel);
        }

        var dto = new AddProjectMVCDto
        {
            ImageFile = addProjectModel.ImageFile,
            Title = addProjectModel.Title,
            Description = addProjectModel.Description,
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
    public async Task<IActionResult> UpdateProject([FromForm] UpdateProjectViewModel updateProjectModel)
    {
        if (!ModelState.IsValid)
        {
            return View(updateProjectModel);
        }

        var dto = new UpdateProjectMVCDto
        {
            Title = updateProjectModel.Title,
            Description = updateProjectModel.Description,
            Id = updateProjectModel.Id,
            ImageFile = updateProjectModel.ImageFile,
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
            return View(updateProjectModel);
        }

        TempData["Message"] = result.SuccessMessage;

        return Redirect("/all-projects");
    }

    [HttpGet]
    [Route("delete-project-{id:int}")]
    public async Task<IActionResult> DeleteProject([FromRoute] int id)
    {
        var entityToDelete = _projects.FirstOrDefault(x => x.Id == id);

        _projects.Remove(entityToDelete);

       return Redirect("/all-projects");
    }


    [HttpGet]
    [Route("make-project-visible-{id:int}")]
    public async Task<IActionResult> MakeProjectVisible([FromRoute] int id)
    {
        var entity = _projects.FirstOrDefault(x => x.Id == id);

        entity.IsVisible = true;

        return Redirect("/all-projects");
    }


    [HttpGet]
    [Route("make-project-invisible-{id:int}")]
    public async Task<IActionResult> MakeProjectInVisible([FromRoute] int id)
    {
        var entity = _projects.FirstOrDefault(x => x.Id == id);

        entity.IsVisible = false;

        return Redirect("/all-projects");
    }
}
