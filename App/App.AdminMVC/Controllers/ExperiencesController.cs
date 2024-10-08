using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ExperiencesController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Experience()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AllExperiences()
    {
        return View();
    }

    [HttpGet]
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
    public async Task<IActionResult> UpdateExperience()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateExperience([FromForm] object updateExperienceModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteExperience()
    {
        return View();
    }
}
