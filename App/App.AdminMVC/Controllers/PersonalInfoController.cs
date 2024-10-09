using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class PersonalInfoController : Controller
{
    [HttpGet]
    [Route("personal-info")]
    public async Task<IActionResult> PersonalInfo()
    {
        return View();
    }

    [HttpGet]
    [Route("add-personal-info")]
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
    [Route("update-personal-info")]
    public async Task<IActionResult> UpdatePersonalInfo()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePersonalInfo([FromForm] object updatePersonalInfoModel)
    {
        return View();
    }
 
}
