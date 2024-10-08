using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class EducationsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Education()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AllEducations()
    {
        return View();
    }

    [HttpGet]
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
    public async Task<IActionResult> UpdateEducation()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEducation([FromForm] object updateEducationModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteEducation()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> MakeEducationVisible()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> MakeEducationInVisible()
    {
        return View();
    }
}
