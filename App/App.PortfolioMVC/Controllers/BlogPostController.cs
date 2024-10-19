using App.Services.PortfolioServices.Abstract;
using App.ViewModels.AdminMvc.EducationsViewModels;
using App.ViewModels.PortfolioMvc.BlogPostsViewModels;
using App.ViewModels.PortfolioMvc.CommentsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
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
}
