using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.EducationsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class EducationsController(IEducationService educationService) : Controller
{

    [HttpGet]
    [Route("all-educations")]
    public async Task<IActionResult> AllEducations()
    {
        var result = await educationService.GetAllEducationsAsync();

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/home/index");
        }

        var models = new List<AllEducationsViewModel>();
        var dtos = result.Value;

           models = dtos
          .Select(item => new AllEducationsViewModel
          {
              Id = item.Id,
              School = item.School,
              Degree = item.Degree,
              StartDate = item.StartDate,
              EndDate = item.EndDate,
              IsVisible = item.IsVisible
          })
          .ToList();

        return View(models);
    }

    [HttpGet]
    [Route("add-education")]
    public async Task<IActionResult> AddEducation()
    {
        return View();
    }

    [HttpPost]
    [Route("add-education")]
    public async Task<IActionResult> AddEducation([FromForm] AddEducationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new AddEducationDto
        {
            Degree = model.Degree,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            School = model.School,
        };

        var result = await educationService.AddEducationAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }
        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-educations");
    }

    [HttpGet]
    [Route("update-education-{id:int}")]
    public async Task<IActionResult> UpdateEducation([FromRoute] int id)
    {
        var result = await educationService.GetEducationByIdAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/all-educations");
        }

        var dto = result.Value;

        var educationToUpdate = new UpdateEducationViewModel
        {
            Id = id,
            School = dto.School,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Degree = dto.Degree,
        };

        return View(educationToUpdate);
    }

    [HttpPost]
    [Route("update-education")]
    public async Task<IActionResult> UpdateEducation([FromForm] UpdateEducationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new UpdateEducationDto
        {
            Id = model.Id,
            School = model.School,
            Degree = model.Degree,
            EndDate = model.EndDate,
            StartDate = model.StartDate,
        };

        var result = await educationService.UpdateEducationAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }
        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-educations");
    }

    [HttpGet]
    [Route("delete-education-{id:int}")]
    public async Task<IActionResult> DeleteEducation([FromRoute] int id)
    {
        var result = await educationService.DeleteEducationAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-educations");
    }


    [HttpGet]
    [Route("change-education-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibility([FromRoute] int id)
    {
        var result = await educationService.ChangeEducationVisibilityAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        else
        {
            TempData["Message"] = result.SuccessMessage;
        }
        return Redirect("/all-educations");
    }
}
