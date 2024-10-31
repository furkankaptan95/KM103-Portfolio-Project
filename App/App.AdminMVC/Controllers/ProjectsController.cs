using App.Core.Authorization;
using App.DTOs.ProjectDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.ProjectsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;

[AuthorizeRolesMvc("admin")]
public class ProjectsController(IProjectAdminService projectService) : Controller
{
    [HttpGet]
    [Route("all-projects")]
    public async Task<IActionResult> AllProjects()
    {
        try
        {
            var result = await projectService.GetAllProjectsAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/");
            }

            var dtos = result.Value;

            var models = dtos
           .Select(item => new AdminAllProjectsViewModel
           {
               Id = item.Id,
               Title = item.Title,
               ImageUrl = item.ImageUrl,
               Description = item.Description,
               IsVisible = item.IsVisible
           })
           .ToList();

            return View(models);
        }
        
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Projeler getirilirken beklenmedik bir hata oluştu..";
            return Redirect("/");
        }
    }

    [HttpGet]
    [Route("add-project")]
    public IActionResult AddProject()
    {
        return View();
    }

    [HttpPost]
    [Route("add-project")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProject([FromForm] AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new AddProjectMVCDto

            {
                ImageFile = model.ImageFile,
                Title = model.Title,
                Description = model.Description,
            };

            var result = await projectService.AddProjectAsync(dto);

            if (!result.IsSuccess)
            {
                ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return View(model);
            }

            TempData["Message"] = result.SuccessMessage;

            return Redirect("/all-projects");
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Proje eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("update-project-{id:int}")]
    public async Task<IActionResult> UpdateProject([FromRoute] int id)
    {
        if (id < 1)
        {
            TempData["ErrorMessage"] = "Geçersiz Proje ID Bilgisi!..";
            return Redirect("/all-projects");
        }

        try
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
        
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Güncellemek istediğiniz Proje bilgileri getirilirken beklenmeyen bir hata oluştu.";
            return Redirect("/all-projects");
        }
    }

    [HttpPost]
    [Route("update-project")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProject([FromForm] UpdateProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
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
                var errorMessage = result.Errors.FirstOrDefault();

                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = errorMessage;
                    return Redirect("/all-projects");
                }

                ViewData["ErrorMessage"] = errorMessage;
                return View(model);
            }

            TempData["Message"] = result.SuccessMessage;
            return Redirect("/all-projects");
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Proje güncellenirken beklenmeyen bir hata oluştu.. Tekrar güncellemeyi deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("delete-project-{id:int}")]
    public async Task<IActionResult> DeleteProject([FromRoute] int id)
    {
        if (id < 1)
        {
            TempData["ErrorMessage"] = "Geçersiz Proje ID Bilgisi!..";
            return Redirect("/all-projects");
        }
        try
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
       
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Proje silinirken beklenmedik bir hata oluştu..";
            return Redirect("/all-projects");
        }
    }


    [HttpGet]
    [Route("change-project-visibility-{id:int}")]
    public async Task<IActionResult> ChangeProjectVisibility([FromRoute] int id)
    {
        if (id < 1)
        {
            TempData["ErrorMessage"] = "Geçersiz Proje ID Bilgisi!..";
            return Redirect("/all-projects");
        }
        try
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
        
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Projenin görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            return Redirect("/all-projects");
        }
    }

}
