using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;

[AllowAnonymous]
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

    [HttpGet]
    public async Task<IActionResult> Index2()
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

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }

	[HttpGet]
	public async Task<IActionResult> DownloadCv()
	{
		try
        {
			var result = await homeService.DownloadCvAsync(); // Servisten dosya verisini al

			if (result.IsSuccess)
			{
				var fileBytes = result.Value;
				var downloadedFileName = "FurkanKaptanCV.pdf";

				return File(fileBytes, "application/pdf", downloadedFileName);
			}
			else
			{
				ViewData["ErrorMessage"] = "CV indirilirken bir problem olu�tu..";
				return Redirect("/");
			}
		}

		catch (Exception)
		{
			ViewData["ErrorMessage"] = "CV indirilirken bir problem olu�tu..";
			return Redirect("/");
		}

	}
}
