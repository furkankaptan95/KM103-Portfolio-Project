using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class PersonalInfoController : Controller
{
    private static readonly PersonalInfoEntity? personalInfoEntity = null;

    [HttpGet]
    [Route("personal-info")]
    public async Task<IActionResult> PersonalInfo()
    {
        if(personalInfoEntity is null)
        {
            TempData["Message"] = "Kişisel Bilgi bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurunuz.";
            return Redirect("/add-personal-info");
        }

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
