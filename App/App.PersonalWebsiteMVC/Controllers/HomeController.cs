using Microsoft.AspNetCore.Mvc;

namespace App.PersonalWebsiteMVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}