using App.Data.Entities;
using App.DTOs.ExperienceDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.ExperiencesViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ExperiencesController(IExperienceService experienceService) : Controller
{
    private static int index = 0;

    private static readonly List<ExperienceEntity> experiences = new List<ExperienceEntity>
    {
        new ExperienceEntity
        {
            Id = ++index,
            Title = "Deneyim 1",
            Company = "Firma 1",
            StartDate = DateTime.Now.AddYears(-10),
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur at justo nec risus malesuada condimentum id condimentum ipsum. Nulla ut erat nec magna ultricies pulvinar ut nec neque. Curabitur nunc dui, ullamcorper sed sodales et, faucibus eget neque. Aenean tincidunt, elit ac fermentum laoreet.",
            EndDate = DateTime.Now,
            IsVisible = true,
        },
         new ExperienceEntity
        {
            Id = ++index,
            Title = "Deneyim 2",
            Company = "Firma 2",
            StartDate = DateTime.Now.AddYears(-10),
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur at justo nec risus malesuada condimentum id condimentum ipsum. Nulla ut erat nec magna ultricies pulvinar ut nec neque. Curabitur nunc dui, ullamcorper sed sodales et, faucibus eget neque. Aenean tincidunt, elit ac fermentum laoreet.",
            EndDate = DateTime.Now.AddYears(-9),
            IsVisible = true,
        },
          new ExperienceEntity
        {
            Id = ++index,
            Title = "Deneyim 3",
            Company = "Firma 3",
            StartDate = DateTime.Now.AddMonths(-8),
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur at justo nec risus malesuada condimentum id condimentum ipsum. Nulla ut erat nec magna ultricies pulvinar ut nec neque. Curabitur nunc dui, ullamcorper sed sodales et, faucibus eget neque. Aenean tincidunt, elit ac fermentum laoreet.",
            EndDate = DateTime.Now.AddMonths(-3),
            IsVisible = true,
        },
    };


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

        if (dtos.Count > 0)
        {
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
        }

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
    public async Task<IActionResult> AddExperience([FromForm] AddExperienceViewModel addExperienceModel)
    {
        if (!ModelState.IsValid)
        {
            return View(addExperienceModel);
        }

        var dto = new AddExperienceDto
        {
            Title = addExperienceModel.Title,
            Company = addExperienceModel.Company,
            StartDate = addExperienceModel.StartDate,
            EndDate = addExperienceModel.EndDate,
            Description = addExperienceModel.Description,
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
    public async Task<IActionResult> UpdateExperience([FromForm] UpdateExperienceViewModel updateExperienceModel)
    {
        if (!ModelState.IsValid)
        {
            return View(updateExperienceModel);
        }

        var dto = new UpdateExperienceDto
        {
            Id = updateExperienceModel.Id,
            Description = updateExperienceModel.Description,
            Title = updateExperienceModel.Title,
            Company = updateExperienceModel.Company,
            EndDate = updateExperienceModel.EndDate,
            StartDate = updateExperienceModel.StartDate,
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
        var entityToDelete = experiences.FirstOrDefault(e=>e.Id == id);

        experiences.Remove(entityToDelete);

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
