using App.Data.Entities;
using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.EducationsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class EducationsController(IEducationService educationService) : Controller
{
    private static int index = 0;

    private static readonly List<EducationEntity> educations = new(); 

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

        if(dtos.Count > 0)
        {
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

        }

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
    public async Task<IActionResult> AddEducation([FromForm] AddEducationViewModel addEducationModel)
    {
        if (!ModelState.IsValid)
        {
            return View(addEducationModel);
        }

        var dto = new AddEducationDto
        {
            Degree = addEducationModel.Degree,
            StartDate = addEducationModel.StartDate,
            EndDate = addEducationModel.EndDate,
            School = addEducationModel.School,
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
    public async Task<IActionResult> UpdateEducation([FromForm] UpdateEducationViewModel updateEducationModel)
    {
        if (!ModelState.IsValid)
        {
            return View(updateEducationModel);
        }

        var dto = new UpdateEducationDto
        {
            Id = updateEducationModel.Id,
            School = updateEducationModel.School,
            Degree = updateEducationModel.Degree,
            EndDate = updateEducationModel.EndDate,
            StartDate = updateEducationModel.StartDate,
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
        var entityToDelete = educations.FirstOrDefault(e => e.Id == id);

        educations.Remove(entityToDelete);

        return Redirect("/all-educations");

    }


    [HttpGet]
    [Route("make-education-visible-{id:int}")]
    public async Task<IActionResult> MakeEducationVisible([FromRoute] int id)
    {
        var entity = educations.FirstOrDefault(e => e.Id == id);

        entity.IsVisible = true;

        return Redirect("/all-educations");
    }


    [HttpGet]
    [Route("make-education-invisible-{id:int}")]
    public async Task<IActionResult> MakeEducationInVisible([FromRoute] int id)
    {
        var entity = educations.FirstOrDefault(e => e.Id == id);

        entity.IsVisible = false;

        return Redirect("/all-educations");
    }
}
