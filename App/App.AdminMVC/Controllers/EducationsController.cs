using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class EducationsController : Controller
{
    private static readonly int index = 0;

    private static readonly List<EducationEntity> educations = new List<EducationEntity>
    {
        new EducationEntity
        {
            Id = ++index,
            Degree = "Lisans",
            School = "İTÜ",
            StartDate = DateTime.Now.AddYears(-10),
            EndDate = DateTime.Now,
        },
         new EducationEntity
        {
            Id = ++index,
            Degree = "Hazırlık Sınıfı",
            School = "İTÜ",
            StartDate = DateTime.Now.AddYears(-10),
            EndDate = DateTime.Now.AddYears(-9),
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
        return View();
    }

    [HttpGet]
    [Route("add-education")]
    public async Task<IActionResult> AddEducation()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddEducation([FromForm] object addEducationModel)
    {
        return View();
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
