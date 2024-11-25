using App.DTOs.PersonalInfoDtos;
using App.DTOs.PersonalInfoDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.PersonalInfoViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;

[Authorize(Roles = "admin")]
public class PersonalInfoController(IPersonalInfoAdminService personalInfoService) : Controller
{
    [HttpGet]
    [Route("personal-info")]
    public async Task<IActionResult> PersonalInfo()
    {
        try
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

                return Redirect("/");
            }

            var dto = result.Value;

            var aboutMeModel = new PersonalInfoViewModel
            {
                Name = dto.Name,
                Surname = dto.Surname,
                About = dto.About,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                Link = dto.Link,
                Adress = dto.Adress,
            };

            return View(aboutMeModel);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Kişisel bilgiler getirilirken beklenmeyen bir hata oluştu.";

            return Redirect("/");
        }
    }

    [HttpGet]
    [Route("add-personal-info")]
    public async Task<IActionResult> AddPersonalInfo()
    {
        try
        {
            var result = await personalInfoService.CheckPersonalInfoAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Ekleme ekranı getirilirken beklenmedik bir hata oluştu..";

                return Redirect("/");
            }

            if (result.Value == false)
            {
                return View();
            }

            TempData["ErrorMessage"] = "Kişisel Bilgiler kısmına daha önceden zaten ekleme yapılmış!..";

            return Redirect("/personal-info");
        }
        catch (Exception)
        {

            TempData["ErrorMessage"] = "Ekleme ekranı getirilirken beklenmedik bir hata oluştu..";

            return Redirect("/");
        }
    }

    [HttpPost]
    [Route("add-personal-info")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPersonalInfo([FromForm] AddPersonalInfoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            var dto = new AddPersonalInfoDto
            {
                About = model.About,
                Name = model.Name,
                BirthDate = model.BirthDate,
                Surname = model.Surname,
                Email = model.Email,
                Link = model.Link,
                Adress = model.Adress,
            };

            var result = await personalInfoService.AddPersonalInfoAsync(dto);

            if (!result.IsSuccess)
            {
                if(result.Status == ResultStatus.Conflict)
                {
                    TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                    return Redirect("/personal-info");
                }

                ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return View(model);
            }

            TempData["Message"] = result.SuccessMessage;

            return Redirect("/personal-info");
        }

        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Kişisel Bilgiler eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("update-personal-info")]
    public async Task<IActionResult> UpdatePersonalInfo()
    {
        try
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
                return Redirect("/personal-info");
            }

            var dto = result.Value;

            var model = new UpdatePersonalInfoViewModel
            {
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
                About = dto.About,
                Adress = dto.Adress,
                Email = dto.Email,
                Link = dto.Link,
            };

            return View(model);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Kişisel Bilgiler getirilirken beklenmedik bir hata oluştu..";
            return Redirect("/personal-info");
        }
    }

    [HttpPost]
    [Route("update-personal-info")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePersonalInfo([FromForm] UpdatePersonalInfoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new UpdatePersonalInfoDto
            {
                Name = model.Name,
                Surname = model.Surname,
                BirthDate = model.BirthDate,
                About = model.About,
                Email = model.Email,
                Adress=model.Adress,
                Link = model.Link,
            };

            var result = await personalInfoService.UpdatePersonalInfoAsync(dto);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault();

                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = errorMessage;
                    return Redirect("/add-personal-info");
                }

                ViewData["ErrorMessage"] = errorMessage;
                return View(model);
            }

            TempData["Message"] = result.SuccessMessage;

            return Redirect("/personal-info");
        }

        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Kişisel bilgiler güncellenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }
 
}
