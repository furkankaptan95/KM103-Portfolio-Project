using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class CommentsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> AllComments()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddComment()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromForm] object addCommentModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteComment()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> ApproveComment()
    {
        return View();
    }

}
