using App.Core;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc.BlogPostsViewModels;
using App.ViewModels.PortfolioMvc.CommentsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
[AllowAnonymousManuel]
public class BlogPostController(IBlogPostPortfolioService blogPostService) : Controller
{
	[HttpGet]
	[Route("blog-post-{id:int}")]
	public async Task<IActionResult> BlogPost([FromRoute] int id)
    {
		BlogPostPagePortfolioViewModel blogPostPageModel = new BlogPostPagePortfolioViewModel();

		SingleBlogPostViewModel model = null;

		var result = await blogPostService.GetBlogPostById(id);

		var dto = result.Value;

		if (result.IsSuccess)
		{
			model = new();

			var commentsModel = dto.Comments
		  .Select(item => new BlogPostCommentsPortfolioViewModel
		  {
			  Id = item.Id,
			  Content = item.Content,
			  Commenter = item.Commenter,
			  CreatedAt = item.CreatedAt,
		  })
		  .ToList();

			model.Id = dto.Id;
			model.Title = dto.Title;
			model.PublishDate = dto.PublishDate;
			model.Content = dto.Content;
			model.Comments = commentsModel;

            blogPostPageModel.BlogPost = model;


            return View(blogPostPageModel);
		}

		if(result.Status == ResultStatus.NotFound)
		{
			model = new();
			blogPostPageModel.BlogPost = model;
			return View(blogPostPageModel);
		}

        return View(blogPostPageModel);
    }

    [HttpGet]
    [Route("all-blog-posts")]
    public async Task<IActionResult> AllBlogPosts()
    {
        List<HomeBlogPostsPortfolioViewModel> models = null;

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
}
