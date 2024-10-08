using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class AboutMeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> AboutMe()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddAboutMe()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddAboutMe([FromForm] object addAboutMeModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> UpdateAboutMe()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAboutMe([FromForm] object updateAboutMeModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteAboutMe()
    {
        return View();
    }
}
