using App.Services.PortfolioServices.Abstract;
using App.ViewModels.AdminMvc.EducationsViewModels;
using App.ViewModels.PortfolioMvc.BlogPostsViewModels;
using App.ViewModels.PortfolioMvc.CommentsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class BlogPostController(IBlogPosPortfolioService blogPostService) : Controller
{
	[HttpGet]
	[Route("blog-post-{id:int}")]
	public async Task<IActionResult> BlogPost([FromRoute] int id)
    {
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

			return View(model);
		}

		if(result.Status == ResultStatus.NotFound)
		{
			model = new();
			return View(model);
		}

        return View(model);
    }
}
