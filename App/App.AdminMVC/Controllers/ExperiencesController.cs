using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ExperiencesController : Controller
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
        return View();
    }

    [HttpGet]
    [Route("add-experience")]
    public async Task<IActionResult> AddExperience()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddExperience([FromForm] object addExperienceModel)
    {
        return View();
    }

    [HttpGet]
    [Route("update-experience-{id:int}")]
    public async Task<IActionResult> UpdateExperience([FromRoute] int id)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateExperience([FromForm] object updateExperienceModel)
    {
        return View();
    }

    [HttpGet]
    [Route("delete-experience-{id:int}")]
    public async Task<IActionResult> DeleteExperience([FromRoute] int id)
    {
        return View();
    }

    [HttpGet]
    [Route("make-experience-visible-{id:int}")]
    public async Task<IActionResult> MakeExperienceVisible([FromRoute] int id)
    {
        return View();
    }

    [HttpGet]
    [Route("make-experience-invisible-{id:int}")]
    public async Task<IActionResult> MakeExperienceInVisible([FromRoute] int id)
    {
        return View();
    }
}
