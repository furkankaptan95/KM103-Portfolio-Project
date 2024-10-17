using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.CommentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class CommentsController(ICommentService commentService) : Controller
{
    [HttpGet]
    [Route("all-comments")]
    public async Task<IActionResult> AllComments()
    {
        try
        {
            var result = await commentService.GetAllCommentsAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/home/index");
            }

            var dtos = result.Value;

            List<AllCommentsViewModel> models = dtos
              .Select(item => new AllCommentsViewModel
              {
                  Id = item.Id,
                  Content = item.Content,
                  CreatedAt = item.CreatedAt,
                  IsApproved = item.IsApproved,
                  BlogPostName = item.BlogPostName,
                  Commenter = item.Commenter,
              })
              .ToList();

            return View(models);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Yorumlar getirilirken beklenmedik bir hata oluştu..";
            return Redirect("/home/index");
        }
    }

    [HttpGet]
    [Route("delete-comment-{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            var result = await commentService.DeleteCommentAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }

            else
            {
                TempData["Message"] = result.SuccessMessage;
            }

            return Redirect("/all-comments");
        }
       
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Yorum silinirken beklenmedik bir hata oluştu..";
            return Redirect("/all-comments");
        }
    }

    [HttpGet]
    [Route("(not)-approve-comment-{id:int}")]
    public async Task<IActionResult> ApproveNotApproveComment([FromRoute] int id)
    {
        try
        {
            var result = await commentService.ApproveOrNotApproveCommentAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }

            else
            {
                TempData["Message"] = result.SuccessMessage;
            }

            return Redirect("/all-comments");
        }

        catch (Exception)
        {
            TempData["ErrorMessage"] = "Yorumun onay durumu değiştirilirken beklenmeyen bir hata oluştu..";
            return Redirect("/all-comments");
        }
    }
}
