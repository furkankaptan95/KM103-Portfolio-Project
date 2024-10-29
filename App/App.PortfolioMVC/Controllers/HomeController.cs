using App.Core.Authorization;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;

[AllowAnonymousManuel]
public class HomeController(IHomePortfolioService homeService) : Controller
{
    [HttpGet]
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
}
