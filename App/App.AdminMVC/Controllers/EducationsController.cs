using App.Data.Entities;
using App.ViewModels.AdminMvc.EducationsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class EducationsController : Controller
{
    private static int index = 0;

    private static readonly List<EducationEntity> educations = new List<EducationEntity>
    {
        new EducationEntity
        {
            Id = ++index,
            Degree = "Lisans",
            School = "İTÜ",
            StartDate = DateTime.Now.AddYears(-10),
            EndDate = DateTime.Now,
            IsVisible = true,
        },
         new EducationEntity
        {
            Id = ++index,
            Degree = "Hazırlık Sınıfı",
            School = "İTÜ",
            StartDate = DateTime.Now.AddYears(-10),
            EndDate = DateTime.Now.AddYears(-9),
            IsVisible = true,
        },
          new EducationEntity
        {
            Id = ++index,
            Degree = "Temel Programlama Eğitimi",
            School = "Siliconmade Academy",
            StartDate = DateTime.Now.AddMonths(-8),
            EndDate = DateTime.Now.AddMonths(-3),
            IsVisible = true,
        },
    };



    [HttpGet]
    [Route("education-{id:int}")]
    public async Task<IActionResult> Education([FromRoute] int id)
    {
        return View();
    }

    [HttpGet]
    [Route("all-educations")]
    public async Task<IActionResult> AllEducations()
    {
        List<AllEducationsViewModel> models = educations
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
    public async Task<IActionResult> AddEducation([FromForm] AddEducationViewModel addEducationModel)
    {
        if (!ModelState.IsValid)
        {
            return View(addEducationModel);
        }

        var educationToAdd = new EducationEntity
        {
            Id = ++index,
            School = addEducationModel.School,
            Degree = addEducationModel.Degree,
            EndDate = addEducationModel.EndDate,
            StartDate = addEducationModel.StartDate,
            IsVisible = true,
        };

        educations.Add(educationToAdd);

        return Redirect("/all-educations");
    }

    [HttpGet]
    [Route("update-education-{id:int}")]
    public async Task<IActionResult> UpdateEducation([FromRoute] int id)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEducation([FromForm] object updateEducationModel)
    {
        return View();
    }

    [HttpGet]
    [Route("delete-education-{id:int}")]
    public async Task<IActionResult> DeleteEducation([FromRoute] int id)
    {
        return View();
    }


    [HttpGet]
    [Route("make-education-visible-{id:int}")]
    public async Task<IActionResult> MakeEducationVisible([FromRoute] int id)
    {
        return View();
    }


    [HttpGet]
    [Route("make-education-invisible-{id:int}")]
    public async Task<IActionResult> MakeEducationInVisible([FromRoute] int id)
    {
        return View();
    }
}
