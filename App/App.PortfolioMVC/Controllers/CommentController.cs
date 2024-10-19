using App.ViewModels.PortfolioMvc.CommentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class CommentController : Controller
{
    [HttpPost]
    public async Task<IActionResult> AddUnsignedComment([FromForm]UnSignedAddCommentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect($"/blog-post-{model.BlogPostId}");
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

        return Redirect($"/blog-post-{model.BlogPostId}");
    }
}
