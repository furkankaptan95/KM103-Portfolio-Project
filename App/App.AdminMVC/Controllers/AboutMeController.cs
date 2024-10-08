using App.ViewModels.AdminMvc.AboutMeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class AboutMeController : Controller
{
  
    [HttpGet]
    public async Task<IActionResult> AboutMe()
    {

        var aboutMeModel = new AboutMeViewModel();

        aboutMeModel.Introduction = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.";
        aboutMeModel.ImageUrl1 = "default-img.jpg";
        aboutMeModel.ImageUrl2 = "default-img.jpg";

        return View(aboutMeModel);
    }

    [HttpGet]
    public async Task<IActionResult> AddAboutMe()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddAboutMe(AddAboutMeViewModel model)
    {
        if(!ModelState.IsValid)
        {
            // Model geçersiz ise view'a geri dön
            return View(model);
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> UpdateAboutMe()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAboutMe([FromForm] object updateAboutMeModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteAboutMe()
    {
        return View();
    }
}
