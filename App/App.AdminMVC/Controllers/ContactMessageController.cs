using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ContactMessageController : Controller
{
    [HttpGet]
    [Route("all-contact-messages")]
    public async Task<IActionResult> AllContactMessages()
    {
        return View();
    }
}
