using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;

public class HomeController(IHomePortfolioService homeService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new HomeIndexViewModel();

       var result = await homeService.GetHomeInfosAsync();

        if (result.IsSuccess)
        {
            model = result.Value;
			return View(model);
		}

		return View(model);
	}

    [HttpPost]
    public async Task<IActionResult> ContactMessage(AddContactMessageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }
}
