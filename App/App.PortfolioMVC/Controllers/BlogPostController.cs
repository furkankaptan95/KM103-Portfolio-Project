using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class BlogPostController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
