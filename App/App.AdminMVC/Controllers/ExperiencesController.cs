using App.Data.Entities;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.EducationsViewModels;
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
        List<AllExperiencesViewModel> models = experiences
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
    public async Task<IActionResult> AddExperience([FromForm] AddExperienceViewModel addExperienceModel)
    {
        if (!ModelState.IsValid)
        {
            return View(addExperienceModel);
        }

        var experienceToAdd = new ExperienceEntity
        {
            Id = ++index,
            Title = addExperienceModel.Title,
            Company = addExperienceModel.Company,
            Description= addExperienceModel.Description,
            EndDate = addExperienceModel.EndDate,
            StartDate = addExperienceModel.StartDate,
            IsVisible = true,
        };

        experiences.Add(experienceToAdd);

        return Redirect("/all-experiences");
    }

    [HttpGet]
    [Route("update-experience-{id:int}")]
    public async Task<IActionResult> UpdateExperience([FromRoute] int id)
    {
        var entityToUpdate = experiences.FirstOrDefault(e => e.Id == id);

        var model = new UpdateExperienceViewModel{
            Id = id,
            Title = entityToUpdate.Title,
            Company = entityToUpdate.Company,
            Description = entityToUpdate.Description,
            EndDate = entityToUpdate.EndDate,
            StartDate = entityToUpdate.StartDate,

        };

        return View(model);
    }

    [HttpPost]
    [Route("update-experience")]
    public async Task<IActionResult> UpdateExperience([FromForm] UpdateExperienceViewModel updateExperienceModel)
    {
        var entityToUpdate = experiences.FirstOrDefault(e => e.Id == updateExperienceModel.Id);

        entityToUpdate.Title = updateExperienceModel.Title;
        entityToUpdate.Company = updateExperienceModel.Company;
        entityToUpdate.Description = updateExperienceModel.Description;
        entityToUpdate.StartDate = updateExperienceModel.StartDate;
        entityToUpdate.EndDate = updateExperienceModel.EndDate;

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
    [Route("make-experience-visible-{id:int}")]
    public async Task<IActionResult> MakeExperienceVisible([FromRoute] int id)
    {
        var entity = experiences.FirstOrDefault(e=>e.Id == id);

        entity.IsVisible = true;

        return Redirect("/all-experiences");
    }

    [HttpGet]
    [Route("make-experience-invisible-{id:int}")]
    public async Task<IActionResult> MakeExperienceInVisible([FromRoute] int id)
    {
        var entity = experiences.FirstOrDefault(e => e.Id == id);

        entity.IsVisible = false;

        return Redirect("/all-experiences");
    }
}
