using App.Data.Entities;
using App.ViewModels.AdminMvc.ExperiencesViewModels;
using App.ViewModels.AdminMvc.ProjectsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ProjectsController : Controller
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
        List<AllProjectsViewModel> models = _projects
    .Select(item => new AllProjectsViewModel
    {
        Id = item.Id,
        Title = item.Title,
        Description = item.Description,
        IsVisible = item.IsVisible,
        ImageUrl = item.ImageUrl,
        
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
    public async Task<IActionResult> AddProject([FromForm] AddProjectViewModel addProjectModel)
    {
        if (!ModelState.IsValid)
        {
            return View(addProjectModel);
        }

        var entityToAdd = new ProjectEntity
        {
            Id = ++index,
            Title = addProjectModel.Title,
            Description = addProjectModel.Description,
            ImageUrl = "default-img.jpg"
        };

        _projects.Add(entityToAdd);

        return Redirect("/all-projects");
    }

    [HttpGet]
    [Route("update-project-{id:int}")]
    public async Task<IActionResult> UpdateProject([FromRoute] int id)
    {
        var entityToUpdate = _projects.FirstOrDefault(item => item.Id == id);

        var model = new UpdateProjectViewModel
        {
            Id = id,
            Title = entityToUpdate.Title,
            Description = entityToUpdate.Description,
            ImageUrl = entityToUpdate.ImageUrl,
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

        var entity = _projects.FirstOrDefault(p=>p.Id == updateProjectModel.Id);

        entity.Title = updateProjectModel.Title;
        entity.Description = updateProjectModel.Description;

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
