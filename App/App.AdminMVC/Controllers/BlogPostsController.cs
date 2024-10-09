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
                Content ="There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
            },
            new AllBlogPostsViewModel
            {
                Id = 2,
                PublishDate = DateTime.Now.AddYears(-1),
                Title = "Post2",
                IsVisible = false,
                Content ="Deneme There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
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
