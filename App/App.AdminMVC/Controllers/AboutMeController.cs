using App.DTOs.AboutMeDtos;
using App.DTOs.AboutMeDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.AboutMeViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class AboutMeController(IAboutMeAdminService aboutMeService) : Controller
{
    [HttpGet]
    [Route("about-me")]
    public async Task<IActionResult> AboutMe()
    {
        try
        {
            var result = await aboutMeService.GetAboutMeAsync();

            if (!result.IsSuccess)
            {
                string errorMessage = result.Errors.FirstOrDefault();

                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["Message"] = errorMessage;
                    return Redirect("/add-about-me");
                }

                TempData["ErrorMessage"] = errorMessage;

                return Redirect("/home/index");
            }

            var dto = result.Value;

            var aboutMeModel = new AdminAboutMeViewModel
            {
                FullName = dto.FullName,
                Field = dto.Field,
                ImageUrl1 = dto.ImageUrl1,
                ImageUrl2 = dto.ImageUrl2,
                Introduction = dto.Introduction,
            };

            return View(aboutMeModel);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Hakkımda kısmındaki bilgiler getirilirken beklenmeyen bir hata oluştu.";

            return Redirect("/home/index");
        }
    }

    [HttpGet]
    [Route("add-about-me")]
    public async Task<IActionResult> AddAboutMe()
    {
        try
        {
            var result = await aboutMeService.CheckAboutMeAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Ekleme ekranı getirilirken beklenmedik bir hata oluştu..";

                return Redirect("/home/index");
            }

            if (result.Value == false)
            {
                return View();
            }

            TempData["ErrorMessage"] = "Hakkımda kısmına daha önceden zaten ekleme yapılmış!..";

            return Redirect("/about-me");
        }
        catch (Exception)
        {

            TempData["ErrorMessage"] = "Ekleme ekranı getirilirken beklenmedik bir hata oluştu..";

            return Redirect("/home/index");
        }
    }

    [HttpPost]
    [Route("add-about-me")]
    public async Task<IActionResult> AddAboutMe([FromForm] AddAboutMeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var mvcDto = new AddAboutMeMVCDto
            {
                Field = model.Field,
                FullName = model.FullName,
                Introduction = model.Introduction,
                ImageFile1 = model.Image1,
                ImageFile2 = model.Image2,
            };

            var result = await aboutMeService.AddAboutMeAsync(mvcDto);

            if (!result.IsSuccess)
            {
                ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return View(model);
            }

            TempData["Message"] = result.SuccessMessage;

            return Redirect("/about-me");
        }

        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Hakkımda kısmı eklenirken beklenmeyen bir hata oluştu.Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("update-about-me")]
    public async Task<IActionResult> UpdateAboutMe()
    {
        try
        {
            var result = await aboutMeService.GetAboutMeAsync();

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault();

                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["Message"] = errorMessage;
                    return Redirect("/add-about-me");
                }

                TempData["ErrorMessage"] = errorMessage;
                return Redirect("/about-me");
            }

            var dto = result.Value;

            var model = new UpdateAboutMeViewModel
            {
                Field = dto.Field,
                FullName = dto.FullName,
                ImageUrl1 = dto.ImageUrl1,
                ImageUrl2 = dto.ImageUrl2,
                Introduction = dto.Introduction,
            };

            return View(model);
        }
        catch (Exception )
        {
            TempData["ErrorMessage"] = "Hakkımda kısmı getirilirken beklenmeyen bir hata oluştu.";
            return Redirect("/about-me");
        }
    }

    [HttpPost]
    [Route("update-about-me")]
    public async Task<IActionResult> UpdateAboutMe([FromForm] UpdateAboutMeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new UpdateAboutMeMVCDto
            {
                FullName = model.FullName,
                Field = model.Field,
                ImageFile1 = model.ImageFile1,
                ImageFile2 = model.ImageFile2,
                Introduction = model.Introduction,
            };

            var result = await aboutMeService.UpdateAboutMeAsync(dto);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault();

                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = errorMessage;
                    return Redirect("/add-about-me");
                }

                ViewData["ErrorMessage"] = errorMessage;
                return View(model);
            }

            TempData["Message"] = result.SuccessMessage;

            return Redirect("/about-me");
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Hakkımda kısmı güncellenirken beklenmeyen bir hata oluştu.Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

}
