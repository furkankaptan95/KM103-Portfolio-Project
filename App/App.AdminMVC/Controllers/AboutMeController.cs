using App.ViewModels.AdminMvc.AboutMeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class AboutMeController : Controller
{
  
    [HttpGet]
    public async Task<IActionResult> AboutMe()
    {
        return View();
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
