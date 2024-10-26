using App.Core.Authorization;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.CommentsViewModels;
using App.ViewModels.AdminMvc.UsersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;

[AuthorizeRolesMvc("admin")]
public class UsersController(IUserAdminService userService) : Controller
{
    [HttpGet]
    [Route("all-users")]
    public async Task<IActionResult> AllUsers()
    {
        try
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
                   IsApproved = c.IsApproved,
               }).ToList()
           })
           .ToList();

            return View(models);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Kullanıcılar getirilirken beklenmedik bir hata oluştu..";
            return Redirect("/home/index");
        }
    }

    [HttpGet]
    [Route("change-user-activeness-{id:int}")]
    public async Task<IActionResult> ChangeUserActiveness([FromRoute] int id)
    {
        try
        {
            var result = await userService.ChangeActivenessOfUserAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }

            else
            {
                TempData["Message"] = result.SuccessMessage;
            }

            return Redirect("/all-users");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Kullanıcının aktifliği değiştirilirken beklenmeyen bir hata oluştu..";
            return Redirect("/all-users");
        }
    }
}
