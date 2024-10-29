using App.Core.Authorization;
using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc.CommentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class CommentController(ICommentPortfolioService commentService) : Controller
{
    [AllowAnonymousManuel]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddUnsignedComment([FromForm]UnSignedAddCommentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect($"/blog-post-{model.BlogPostId}");
        }

        try
        {
            var dto = new AddCommentUnsignedDto
            {
                BlogPostId = model.BlogPostId,
                UnsignedCommenterName = model.UnsignedCommenterName,
                Content = model.Content,
            };

            var result = await commentService.AddCommentUnsignedAsync(dto);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.Errors.First();
            }

            return Redirect($"/blog-post-{model.BlogPostId}");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Yorumunuz alınırken beklenmedik bir hata oluştu..";
            return Redirect($"/blog-post-{model.BlogPostId}");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSignedComment([FromForm] SignedAddCommentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect($"/blog-post-{model.BlogPostId}");
        }

        try
        {
            var dto = new AddCommentSignedDto
            {
                BlogPostId = model.BlogPostId,
                UserId = model.UserId,
                Content = model.Content,
            };

            var result = await commentService.AddCommentSignedAsync(dto);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.Errors.First();
            }

            return Redirect($"/blog-post-{model.BlogPostId}");
        }

        catch (Exception)
        {
            TempData["ErrorMessage"] = "Yorumunuz alınırken beklenmedik bir hata oluştu..";
            return Redirect($"/blog-post-{model.BlogPostId}");
        }
    }
}
