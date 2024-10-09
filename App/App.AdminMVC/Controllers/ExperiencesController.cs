using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ExperiencesController : Controller
{
    [HttpGet]
    [Route("experience-{id:int}")]
    public async Task<IActionResult> Experience([FromRoute] int id)
    {
        return View();
    }

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
