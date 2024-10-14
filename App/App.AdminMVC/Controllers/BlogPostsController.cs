using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.BlogPostsViewModels;
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
            var errorMessage = result.Errors.FirstOrDefault();
            TempData["ErrorMessage"] = errorMessage;

            return Redirect("/home/index");
        }

        var dtos = result.Value;

        if(dtos.Count > 0)
        {
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
        }

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

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        TempData["Message"] = result.SuccessMessage;

        return Redirect("/all-blog-posts");

    }

    [HttpGet]
    [Route("update-blog-post-{id:int}")]
    public async Task<IActionResult> UpdateBlogPost([FromRoute] int id)
    {
        var result = await blogPostService.GetBlogPostById(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/all-blog-posts");
        }

        var dto = result.Value;

        var model = new UpdateBlogPostViewModel()
        {
            Id = id,
            Content = dto.Content,
            Title = dto.Title,
        };

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

        var dto = new UpdateBlogPostDto
        {
            Content = updateBlogPostModel.Content,
            Title = updateBlogPostModel.Title,
            Id = updateBlogPostModel.Id
        };

        var result = await blogPostService.UpdateBlogPostAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/all-blog-posts");
        }

        TempData["Message"] = result.SuccessMessage;
        return Redirect("/all-blog-posts");
    }

    [HttpGet]
    [Route("delete-blog-post-{id:int}")]
    public async Task<IActionResult> DeleteBlogPost([FromRoute] int id)
    {
        var result = await blogPostService.DeleteBlogPostAsync(id);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        else
        {
            TempData["Message"] = result.SuccessMessage;
        }

        return Redirect("/all-blog-posts");
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
