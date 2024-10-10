using App.Data.Entities;
using App.ViewModels.AdminMvc.CommentsViewModels;
using App.ViewModels.AdminMvc.EducationsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class CommentsController : Controller
{
    private static readonly List<CommentEntity> _comments = new()
    {
        new()
        {
            Id = 1,
            Content = "İçerik 1",
            CreatedAt = DateTime.Now.AddDays(-4),
            IsApproved = true,
            BlogPostId = 1,
            UnsignedCommenterName = "Yorumcu 1"
        }, new()
        {
            Id = 2,
            Content = "İçerik 2",
            CreatedAt = DateTime.Now.AddDays(-5),
            IsApproved = true,
            BlogPostId = 2,
            UnsignedCommenterName = "Yorumcu 2"
        }, new()
        {
            Id = 3,
            Content = "İçerik 3",
            CreatedAt = DateTime.Now.AddDays(-6),
            IsApproved = false,
            BlogPostId = 3,
            UnsignedCommenterName = "Yorumcu 3"
        },
    };

    [HttpGet]
    [Route("all-comments")]
    public async Task<IActionResult> AllComments()
    {
        List<AllCommentsViewModel> models = _comments
      .Select(item => new AllCommentsViewModel
      {
          Id = item.Id,
          Content = item.Content,
          CreatedAt = item.CreatedAt,
          IsApproved = item.IsApproved,
          BlogPostName = "Neden Backend?",
          Commenter = item.UnsignedCommenterName,
      })
      .ToList();


        return View(models);
    }

    [HttpGet]
    [Route("delete-comment-{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        var comment = _comments.FirstOrDefault(c=>c.Id == id);

        _comments.Remove(comment);

        return Redirect("all-comments");
    }

    [HttpGet]
    [Route("approve-comment-{id:int}")]
    public async Task<IActionResult> ApproveComment([FromRoute] int id)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == id);

        comment.IsApproved = true;

        return Redirect("all-comments");
    }

    [HttpGet]
    [Route("not-approve-comment-{id:int}")]
    public async Task<IActionResult> NotApproveComment([FromRoute] int id)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == id);

        comment.IsApproved = false;

        return Redirect("all-comments");
    }

}
