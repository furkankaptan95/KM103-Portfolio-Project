using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.BlogPostsViewModels;
using App.ViewModels.AdminMvc.ExperiencesViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class BlogPostsController(IBlogPostService blogPostService) : Controller
{

    [HttpGet]
    [Route("all-blog-posts")]
    public async Task<IActionResult> AllBlogPosts()
    {
        var models = new List<AllBlogPostsViewModel>();

        var result = await blogPostService.GetAllBlogPostsAsync();

        if (!result.IsSuccess)
        {
            ViewData["ErrorMessage"] = "Blog Postlar getirilirken beklenmedik bir hata oluştu.";
            return View(models);
        }

        var dtos = result.Value;

         models = dtos
        .Select(item => new AllBlogPostsViewModel
         {
            Id = item.Id,
            Title = item.Title,
            Content = item.Content,
            PublishDate = item.PublishDate,
            IsVisible = item.IsVisible,
         })
        .ToList();

        return View(models);
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
        if (!ModelState.IsValid)
        {
            return View(addBlogPostModel);
        }

        var dto = new AddBlogPostDto
        {
            Content = addBlogPostModel.Content,
            Title = addBlogPostModel.Title,
        };

        var result = await blogPostService.AddBlogPostAsync(dto);

        if (result.Status == ResultStatus.Invalid)
        {
            ViewBag.ErrorMessage = result.ValidationErrors.FirstOrDefault();
            return View(addBlogPostModel);
        }

        if (result.Status == ResultStatus.Error)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return View("/all-blog-posts");
        }

        TempData["Message"] = result.SuccessMessage;

        return Redirect("/all-blog-posts");

    }

    [HttpGet]
    [Route("update-blog-post-{id:int}")]
    public async Task<IActionResult> UpdateBlogPost([FromRoute] int id)
    {
        var model = new UpdateBlogPostViewModel();

        if(id == 1)
        {
            model.Title = "Post1";
            model.Content = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.";
            model.Id = id;
        }

        if (id == 2)
        {
            model.Title = "Post2";
            model.Content = "Deneme There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.";
            model.Id = id;
        }

        return View(model);
    }

    [HttpPost]
    [Route("update-blog-post")]
    public async Task<IActionResult> UpdateBlogPost([FromForm] UpdateBlogPostViewModel updateBlogPostModel)
    {
        if (!ModelState.IsValid)
        {
            return View(updateBlogPostModel);
        }

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
