using App.Data.Entities;
using App.ViewModels.AdminMvc.PersonalInfoViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class PersonalInfoController : Controller
{
    private static readonly PersonalInfoEntity personalInfoEntity = new();

    [HttpGet]
    [Route("personal-info")]
    public async Task<IActionResult> PersonalInfo()
    {
        if(personalInfoEntity.Id == 0)
        {
            TempData["Message"] = "Kişisel Bilgi bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurunuz.";
            return Redirect("/add-personal-info");
        }

        var model = new PersonalInfoViewModel
        {
            Name = personalInfoEntity.Name,
            Surname = personalInfoEntity.Surname,
            BirthDate = personalInfoEntity.BirthDate,
            About = personalInfoEntity.About,
        };

        return View(model);
    }

    [HttpGet]
    [Route("add-personal-info")]
    public async Task<IActionResult> AddPersonalInfo()
    {
        return View();
    }

    [HttpPost]
    [Route("add-personal-info")]
    public async Task<IActionResult> AddPersonalInfo([FromForm] AddPersonalInfoViewModel addPersonalInfoModel)
    {
        if (!ModelState.IsValid)
        {
            return View(addPersonalInfoModel);
        }

        personalInfoEntity.Surname = addPersonalInfoModel.Surname;
        personalInfoEntity.Name = addPersonalInfoModel.Name;
        personalInfoEntity.About = addPersonalInfoModel.About;
        personalInfoEntity.BirthDate = addPersonalInfoModel.BirthDate;
        personalInfoEntity.Id = 1;

        return Redirect("/personal-info");
    }

    [HttpGet]
    [Route("update-personal-info")]
    public async Task<IActionResult> UpdatePersonalInfo()
    {
        var model = new UpdatePersonalInfoViewModel
        {
            Name = personalInfoEntity.Name,
            Surname = personalInfoEntity.Surname,
            BirthDate = personalInfoEntity.BirthDate,
            About = personalInfoEntity.About,
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePersonalInfo([FromForm] object updatePersonalInfoModel)
    {
        return View();
    }
 
}
