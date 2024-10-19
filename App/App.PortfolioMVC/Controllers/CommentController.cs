using App.ViewModels.PortfolioMvc.CommentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class CommentController : Controller
{
    [HttpPost]
    public async Task<IActionResult> AddComment([FromForm]UnSignedAddCommentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect($"/blog-post-{model.BlogPostId}");
        }

        return Redirect($"/blog-post-{model.BlogPostId}");
    }
}
