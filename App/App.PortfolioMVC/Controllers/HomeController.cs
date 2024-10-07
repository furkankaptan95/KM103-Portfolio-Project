using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}
