using App.ViewModels.AdminMvc.CommentsViewModels;
using App.ViewModels.AdminMvc.UsersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class UsersController : Controller
{
    private static readonly List<AllUsersViewModel> _users = new()
    {
        new()
        {
            Username = "user 1",
            Email = "mail1@gmail.com",
            ImageUrl = "default-img.jpg",
            IsActive = true,
            Comments = new List<UsersCommentsViewModel>()
            {
                new()
                {
                    Content ="Harika bir yazı!",
                    CreatedAt = DateTime.Now.AddDays(-2),
                    IsApproved = true,
                    BlogPostName = "Nasıl Başladım?",
                }, new()
                {
                    Content ="Tebrikler, harika bir değerlendirme olmuş.",
                    CreatedAt = DateTime.Now.AddDays(-22),
                    IsApproved = true,
                    BlogPostName = "Neden Backend?",
                },

            }

        },

          new()
        {
            Username = "user 2",
            Email = "mail2@gmail.com",
            ImageUrl = "default-img.jpg",
            IsActive = true,
            Comments = new List<UsersCommentsViewModel>()
            {
                new()
                {
                    Content ="Bazı fikirlerine katılmasam da güzel bir yazı olmuş!",
                    CreatedAt = DateTime.Now.AddDays(-12),
                    IsApproved = true,
                    BlogPostName = "Nasıl Başladım?",
                }, new()
                {
                    Content ="Harika bir değerlendirme olmuş.",
                    CreatedAt = DateTime.Now.AddDays(-52),
                    IsApproved = true,
                    BlogPostName = "Neden Backend?",
                },

            }

        },
    };

    [HttpGet]
    [Route("all-users")]
    public async Task<IActionResult> AllUsers()
    {
        return View(_users);
    }

    [HttpGet]
    [Route("delete-user-{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        return View();
    }

}
