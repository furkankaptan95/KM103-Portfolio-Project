using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.CommentsViewModels;
using App.ViewModels.AdminMvc.ExperiencesViewModels;
using App.ViewModels.AdminMvc.UsersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class UsersController(IUserService userService) : Controller
{
    private static readonly List<AllUsersViewModel> _users = new()
    {
        new()
        {
            Id = 1,
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
                    IsApproved = false,
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
            Id = 2,
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
        var result = await userService.GetAllUsersAsync();

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/home/index");
        }

        var models = new List<AllUsersViewModel>();
        var dtos = result.Value;

            models = dtos
           .Select(item => new AllUsersViewModel
           {
               Id = item.Id,
               Username = item.Username,
               Email = item.Email,
               IsActive = item.IsActive,
               ImageUrl = item.ImageUrl,
               Comments = item.Comments.Select(c => new UsersCommentsViewModel
               {
                   Content = c.Content,
                   CreatedAt = c.CreatedAt,
                   BlogPostName = c.BlogPostName,
                   IsApproved= c.IsApproved,
               }).ToList()
           })
           .ToList();
        
        return View(models);
    }

    [HttpGet]
    [Route("activate-user-{id:int}")]
    public async Task<IActionResult> ActivateUser([FromRoute] int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        user.IsActive = true;

        return Redirect("/all-users");
    }

    [HttpGet]
    [Route("deactivate-user-{id:int}")]
    public async Task<IActionResult> DeactivateUser([FromRoute] int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        user.IsActive = false;

        return Redirect("/all-users");
    }

}
