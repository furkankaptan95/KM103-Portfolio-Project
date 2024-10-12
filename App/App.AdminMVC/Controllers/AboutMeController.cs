using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.AboutMeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class AboutMeController(IAboutMeService aboutMeService) : Controller
{
  
    [HttpGet]
    [Route("about-me")]
    public async Task<IActionResult> AboutMe()
    {
        var result = await aboutMeService.GetAboutMeAsync();

        if (!result.IsSuccess)
        {
            TempData["Message"] = "Hakkımda bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurunuz.";
            return Redirect("/add-about-me");
        }

        var dto = result.Value;

        var aboutMeModel = new AboutMeViewModel
        {
            ImageUrl1 = dto.ImageUrl1,
            ImageUrl2 = dto.ImageUrl2,
            Introduction = dto.Introduction,
        };

        return View(aboutMeModel);
    }

    [HttpGet]
    [Route("add-about-me")]
    public async Task<IActionResult> AddAboutMe()
    {
        return View();
    }

    [HttpPost]
    [Route("add-about-me")]
    public async Task<IActionResult> AddAboutMe([FromForm] AddAboutMeViewModel model)
    {
       
        var mvcDto = new AddAboutMeMVCDto
        {
            Introduction = model.Introduction,
            ImageFile1 = model.Image1,
            ImageFile2 = model.Image2,
        };

        var result = await aboutMeService.AddAboutMeAsync(mvcDto);

        if (!result.IsSuccess)
        {
            return BadRequest();
        }

        return Redirect("/about-me");
    }

    [HttpGet]
    [Route("update-about-me")]
    public async Task<IActionResult> UpdateAboutMe()
    {
        var result = await aboutMeService.GetAboutMeAsync();

        if (!result.IsSuccess)
        {
            return BadRequest("Hakkımda bilgileri getirilirken bir hata oluştu.");
        }

        var dto = result.Value;

        var model = new UpdateAboutMeViewModel
        {
            ImageUrl1 = dto.ImageUrl1,
            ImageUrl2 = dto.ImageUrl2,
            Introduction = dto.Introduction,
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAboutMe([FromForm] UpdateAboutMeViewModel updateAboutMeModel)
    {
        if (!ModelState.IsValid)
        {
            return View(updateAboutMeModel);
        }

        var dto = new UpdateAboutMeMVCDto
        {
            ImageFile1 = updateAboutMeModel.ImageFile1,
            ImageFile2 = updateAboutMeModel.ImageFile2,
            Introduction = updateAboutMeModel.Introduction,
        };

        var result = await aboutMeService.UpdateAboutMeAsync(dto);

        return View();
    }

}
