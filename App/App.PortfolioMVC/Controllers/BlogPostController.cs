using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc.BlogPostsViewModels;
using App.ViewModels.PortfolioMvc.CommentsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
[AllowAnonymous]
public class BlogPostController(IBlogPostPortfolioService blogPostService) : Controller
{
	[HttpGet]
	[Route("blog-post-{id:int}")]
	public async Task<IActionResult> BlogPost([FromRoute] int id)
    {
        if (id < 1)
        {
            TempData["ErrorMessage"] = "BlogPost bulunamadı.";
            return Redirect("/all-blog-posts");
        }

		BlogPostPagePortfolioViewModel blogPostPageModel = new BlogPostPagePortfolioViewModel();

		SingleBlogPostViewModel model;

        try
        {
            var result = await blogPostService.GetBlogPostById(id);

            if (result.IsSuccess)
            {
                var dto = result.Value;

                model = new();

                List<BlogPostCommentsPortfolioViewModel> commentsModel = null;

                if (dto.Comments is not null)
                {
                    commentsModel = new();

                     commentsModel = dto.Comments
                    .Select(item => new BlogPostCommentsPortfolioViewModel
                    {
                        Id = item.Id,
                        CommenterId = item.CommenterId,
                        Content = item.Content,
                        Commenter = item.Commenter,
                        CreatedAt = item.CreatedAt,
                    })
                    .ToList();
                } 

                model.Id = dto.Id;
                model.Title = dto.Title;
                model.PublishDate = dto.PublishDate;
                model.Content = dto.Content;
                model.Comments = commentsModel;

                blogPostPageModel.BlogPost = model;

                return View(blogPostPageModel);
            }

            if (result.Status == ResultStatus.NotFound)
            {
                model = new();
                blogPostPageModel.BlogPost = model;
                return View(blogPostPageModel);
            }

            return View(blogPostPageModel);
        }

        catch (Exception)
        {
            return View(blogPostPageModel);
        }
    }

	
    [HttpGet]
    [Route("all-blog-posts")]
    public async Task<IActionResult> AllBlogPosts()
    {
        List<HomeBlogPostsPortfolioViewModel> models = null;

		try
		{
            var result = await blogPostService.GetHomeBlogPostsAsync();

            if (!result.IsSuccess)
            {
                return View(models);
            }

            models = new();

            foreach (var blogPost in result.Value)
            {
                var blogPostToAdd = new HomeBlogPostsPortfolioViewModel();

                blogPostToAdd.Title = blogPost.Title;
                blogPostToAdd.Content = blogPost.Content;
                blogPostToAdd.Id = blogPost.Id;
                blogPostToAdd.PublishDate = blogPost.PublishDate;
                blogPostToAdd.CommentsCount = blogPost.CommentsCount;

                models.Add(blogPostToAdd);
            }

            return View(models);
        }

        catch (Exception)
        {
            return View(models);
        }
    }
}
