using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class PersonalInfoController : Controller
{
    [HttpGet]
    public async Task<IActionResult> PersonalInfo()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddPersonalInfo()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddPersonalInfo([FromForm] object addPersonalInfoModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> UpdatePersonalInfo()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePersonalInfo([FromForm] object updatePersonalInfoModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeletePersonalInfo()
    {
        return View();
    }
}
