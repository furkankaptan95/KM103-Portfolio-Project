using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class BlogPostsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> BlogPost()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AllBlogPosts()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddBlogPost()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddBlogPost([FromForm] object addBlogPostModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> UpdateBlogPost()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBlogPost([FromForm] object updateBlogPostModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteBlogPost()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> MakeBlogPostVisible()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> MakeBlogPostInVisible()
    {
        return View();
    }
}
