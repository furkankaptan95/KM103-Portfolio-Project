using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class AuthController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel registerModel)
    {
        return View();
    }
}
