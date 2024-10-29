using App.Core.Authorization;
using App.DTOs.BlogPostDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.BlogPostsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;

[AuthorizeRolesMvc("admin")]
public class BlogPostsController(IBlogPostAdminService blogPostService) : Controller
{
    [HttpGet]
    [Route("all-blog-posts")]
    public async Task<IActionResult> AllBlogPosts()
    {
        try
        {
            var models = new List<AdminAllBlogPostsViewModel>();

            var result = await blogPostService.GetAllBlogPostsAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/home/index");
            }

            var dtos = result.Value;

            models = dtos
           .Select(item => new AdminAllBlogPostsViewModel
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
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Blog Postlar getirilirken beklenmedik bir hata oluştu.";
            return Redirect("/home/index");
        }
    }

    [HttpGet]
    [Route("add-blog-post")]
    public IActionResult AddBlogPost()
    {
        return View();
    }

    [HttpPost]
    [Route("add-blog-post")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBlogPost([FromForm] AddBlogPostViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new AddBlogPostDto
            {
                Content = model.Content,
                Title = model.Title,
            };

            var result = await blogPostService.AddBlogPostAsync(dto);

            if (!result.IsSuccess)
            {
               ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
               return View(model);
            }
                
            TempData["Message"] = result.SuccessMessage; 
            return Redirect("/all-blog-posts");
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Blog Post eklenirken beklenmedik bir hata oluştu..Tekrar eklemeyi deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("update-blog-post-{id:int}")]
    public async Task<IActionResult> UpdateBlogPost([FromRoute] int id)
    {
        try
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
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Blog Post verisi alınırken bir hata oluştu.";
            return Redirect("/all-blog-posts");
        }
    }

    [HttpPost]
    [Route("update-blog-post")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateBlogPost([FromForm] UpdateBlogPostViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new UpdateBlogPostDto
            {
                Id = model.Id,
                Content = model.Content,
                Title = model.Title,
            };

            var result = await blogPostService.UpdateBlogPostAsync(dto);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault();

                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = errorMessage;
                    return Redirect("/all-blog-posts");
                }
                ViewData["ErrorMessage"] = errorMessage;
                return View(model);
            }
                TempData["Message"] = result.SuccessMessage;
                return Redirect("/all-blog-posts");
            
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("delete-blog-post-{id:int}")]
    public async Task<IActionResult> DeleteBlogPost([FromRoute] int id)
    {
        try
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
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Blog Post silinirken beklenmedik bir hata oluştu..";
            return Redirect("/all-blog-posts");
        }

    }

    [HttpGet]
    [Route("change-blog-post-visibility-{id:int}")]
    public async Task<IActionResult> MakeBlogPostVisible([FromRoute] int id)
    {
        try
        {
            var result = await blogPostService.ChangeBlogPostVisibilityAsync(id);

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
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Blog Post'un görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            return Redirect("/all-blog-posts");
        }
    }
}
