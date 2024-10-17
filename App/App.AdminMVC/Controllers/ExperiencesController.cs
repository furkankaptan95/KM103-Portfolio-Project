using App.DTOs.ExperienceDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.ExperiencesViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ExperiencesController(IExperienceService experienceService) : Controller
{
    [HttpGet]
    [Route("all-experiences")]
    public async Task<IActionResult> AllExperiences()
    {
        var result = await experienceService.GetAllExperiencesAsync();

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/home/index");
        }

        var models = new List<AllExperiencesViewModel>();
        var dtos = result.Value;

            models = dtos
           .Select(item => new AllExperiencesViewModel
           {
               Id = item.Id,
               Title = item.Title,
               Company = item.Company,
               Description = item.Description,
               StartDate = item.StartDate,
               EndDate = item.EndDate,
               IsVisible = item.IsVisible
           })
           .ToList();

        return View(models);
    }

    [HttpGet]
    [Route("add-experience")]
    public async Task<IActionResult> AddExperience()
    {
        return View();
    }

    [HttpPost]
    [Route("add-experience")]
    public async Task<IActionResult> AddExperience([FromForm] AddExperienceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new AddExperienceDto
        {
            Title = model.Title,
            Company = model.Company,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Description = model.Description,
        };

        var result = await experienceService.AddExperienceAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }
        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-experiences");
    }

    [HttpGet]
    [Route("update-experience-{id:int}")]
    public async Task<IActionResult> UpdateExperience([FromRoute] int id)
    {
        var result = await experienceService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/all-experiences");
        }

        var dto = result.Value;

        var model = new UpdateExperienceViewModel
        {
            Id = id,
            Company = dto.Company,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Title = dto.Title,
            Description = dto.Description,
        };

        return View(model);
    }

    [HttpPost]
    [Route("update-experience")]
    public async Task<IActionResult> UpdateExperience([FromForm] UpdateExperienceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new UpdateExperienceDto
        {
            Id = model.Id,
            Description = model.Description,
            Title = model.Title,
            Company = model.Company,
            EndDate = model.EndDate,
            StartDate = model.StartDate,
        };

        var result = await experienceService.UpdateExperienceAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }
        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-experiences");
    }

    [HttpGet]
    [Route("delete-experience-{id:int}")]
    public async Task<IActionResult> DeleteExperience([FromRoute] int id)
    {
        var result = await experienceService.DeleteExperienceAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-experiences");
    }

    [HttpGet]
    [Route("change-experience-visibility-{id:int}")]
    public async Task<IActionResult> ChangeExperienceVisibility([FromRoute] int id)
    {
        var result = await experienceService.ChangeExperienceVisibilityAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-experiences");
    }

}
