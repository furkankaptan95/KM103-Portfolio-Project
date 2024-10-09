using App.ViewModels.AdminMvc.BlogPostsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class BlogPostsController : Controller
{
    [HttpGet]
    [Route("blog-post-{id:int}")]
    public async Task<IActionResult> BlogPost([FromRoute] int id)
    {
        return View();
    }

    [HttpGet]
    [Route("all-blog-posts")]
    public async Task<IActionResult> AllBlogPosts()
    {
        var model = new List<AllBlogPostsViewModel>
        {
            new AllBlogPostsViewModel
            {
                Id = 1,
                PublishDate = DateTime.Now.AddYears(-2),
                Title = "Post1",
                IsVisible = true,
            },
            new AllBlogPostsViewModel
            {
                Id = 2,
                PublishDate = DateTime.Now.AddYears(-1),
                Title = "Post2",
                IsVisible = false,
            },

        };


        return View(model);
    }

    [HttpGet]
    [Route("add-blog-post")]
    public async Task<IActionResult> AddBlogPost()
    {
        return View();
    }

    [HttpPost]
    [Route("add-blog-post")]
    public async Task<IActionResult> AddBlogPost([FromForm] AddBlogPostViewModel addBlogPostModel)
    {
        return View();
    }

    [HttpGet]
    [Route("update-blog-post-{id:int}")]
    public async Task<IActionResult> UpdateBlogPost([FromRoute] int id)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBlogPost([FromForm] object updateBlogPostModel)
    {
        return View();
    }

    [HttpGet]
    [Route("delete-blog-post-{id:int}")]
    public async Task<IActionResult> DeleteBlogPost([FromRoute] int id)
    {
        return View();
    }

    [HttpGet]
    [Route("make-blog-post-visible-{id:int}")]
    public async Task<IActionResult> MakeBlogPostVisible([FromRoute] int id)
    {
        return View();
    }

    [HttpGet]
    [Route("make-blog-post-invisible-{id:int}")]
    public async Task<IActionResult> MakeBlogPostInVisible([FromRoute] int id)
    {
        return View();
    }
}
