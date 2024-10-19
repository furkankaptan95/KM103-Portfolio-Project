using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc.CommentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class CommentController(ICommentPortfolioService commentService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> AddUnsignedComment([FromForm]UnSignedAddCommentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect($"/blog-post-{model.BlogPostId}");
        }

        var dto = new AddCommentUnsignedDto
        {
            BlogPostId = model.BlogPostId,
            UnsignedCommenterName = model.UnsignedCommenterName,
            Content = model.Content,
        };

        var result = await commentService.AddCommentUnsignedAsync(dto);

        if (result.IsSuccess)
        {
            TempData["Message"] = result.SuccessMessage;
        }
        else
        {
            TempData["ErrorMessage"] = result.Errors.First();
        }

        return Redirect($"/blog-post-{model.BlogPostId}");
    }

    [HttpPost]
    public async Task<IActionResult> AddSignedComment([FromForm] SignedAddCommentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect($"/blog-post-{model.BlogPostId}");
        }

        var dto = new AddCommentSignedDto
        {
            BlogPostId = model.BlogPostId,
            UserId = model.UserId,
            Content = model.Content,
        };

        var result = await commentService.AddCommentSignedAsync(dto);

        if (result.IsSuccess)
        {
            TempData["Message"] = result.SuccessMessage;
        }
        else
        {
            TempData["ErrorMessage"] = result.Errors.First();
        }

        return Redirect($"/blog-post-{model.BlogPostId}");
    }
}
