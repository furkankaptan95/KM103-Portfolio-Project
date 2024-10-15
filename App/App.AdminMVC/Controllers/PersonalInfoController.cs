using App.Data.Entities;
using App.DTOs.AboutMeDtos;
using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.AboutMeViewModels;
using App.ViewModels.AdminMvc.PersonalInfoViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class PersonalInfoController(IPersonalInfoService personalInfoService) : Controller
{
    private static readonly PersonalInfoEntity personalInfoEntity = new();

    [HttpGet]
    [Route("personal-info")]
    public async Task<IActionResult> PersonalInfo()
    {
        var result = await personalInfoService.GetPersonalInfoAsync();

        if (!result.IsSuccess)
        {
            string errorMessage = result.Errors.FirstOrDefault();

            if (result.Status == ResultStatus.NotFound)
            {
                TempData["Message"] = errorMessage;
                return Redirect("/add-personal-info");
            }

            TempData["ErrorMessage"] = errorMessage;

            return Redirect("/home/index");
        }

        var dto = result.Value;

        var aboutMeModel = new PersonalInfoViewModel
        {
            Name = dto.Name,
            Surname = dto.Surname,
            About = dto.About,
            BirthDate = dto.BirthDate,
        };

        return View(aboutMeModel);
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
