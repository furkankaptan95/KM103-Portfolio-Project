using App.Data.Entities;
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


        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteComment()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> ApproveComment()
    {
        return View();
    }

}
