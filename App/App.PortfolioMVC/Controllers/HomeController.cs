using App.Core.Authorization;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
				TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
				return Redirect("/");
			}
		}

		catch (Exception)
		{
			TempData["ErrorMessage"] = "CV indirilrken bir problem oluþtu..";
			return Redirect("/");
		}

	}
}
