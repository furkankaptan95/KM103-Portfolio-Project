using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.PersonalInfoViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class PersonalInfoController(IPersonalInfoService personalInfoService) : Controller
{
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
        var result = await personalInfoService.GetPersonalInfoAsync();

        if (!result.IsSuccess)
        {
            var errorMessage = result.Errors.FirstOrDefault();

            if (result.Status == ResultStatus.NotFound)
            {
                TempData["ErrorMessage"] = errorMessage;
                return Redirect("/add-personal-info");
            }

            TempData["ErrorMessage"] = errorMessage;
            return Redirect("/home/index");
        }

        var dto = result.Value;

        var model = new UpdatePersonalInfoViewModel
        {
            Name = dto.Name,
            Surname = dto.Surname,
            BirthDate = dto.BirthDate,
            About = dto.About,
        };

        return View(model);
    }

    [HttpPost]
    [Route("update-personal-info")]
    public async Task<IActionResult> UpdatePersonalInfo([FromForm] UpdatePersonalInfoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new UpdatePersonalInfoDto
        {
           Name = model.Name,
           Surname = model.Surname,
           BirthDate = model.BirthDate,
           About = model.About,
        };

        var result = await personalInfoService.UpdatePersonalInfoAsync(dto);

        if (!result.IsSuccess)
        {
            if(result.Status == ResultStatus.NotFound)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/add-personal-info");
            }

            ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return View(model);
        }

        TempData["Message"] = result.SuccessMessage;

        return Redirect("/personal-info");
    }
 
}
