using App.Data.Entities;
using App.DTOs.AboutMeDtos;
using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.PersonalInfoViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class PersonalInfoController(IPersonalInfoService personalInfoService) : Controller
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
    public async Task<IActionResult> AddPersonalInfo([FromForm] AddPersonalInfoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new AddPersonalInfoDto
        {
            About = model.About,
            Name = model.Name,
            BirthDate = model.BirthDate,
            Surname = model.Surname,
        };

        var result = await personalInfoService.AddPersonalInfoAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/home/index");
        }

        TempData["Message"] = result.SuccessMessage;

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
    [Route("update-personal-info")]
    public async Task<IActionResult> UpdatePersonalInfo([FromForm] UpdatePersonalInfoViewModel updatePersonalInfoModel)
    {
        personalInfoEntity.Name = updatePersonalInfoModel.Name;
        personalInfoEntity.Surname = updatePersonalInfoModel.Surname;
        personalInfoEntity.About = updatePersonalInfoModel.About;
        personalInfoEntity.BirthDate = updatePersonalInfoModel.BirthDate;

        return Redirect("/personal-info");
    }
 
}
