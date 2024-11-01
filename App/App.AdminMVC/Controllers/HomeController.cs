using App.Core.Authorization;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;

public class HomeController(IHomeAdminService homeService) : Controller
{

    [AllowAnonymousManuel]

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }

    [AuthorizeRolesMvc("admin")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await homeService.GetHomeInfosAsync();

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index2));
            }

            var dto = result.Value;

            var model = new HomeViewModel
            {
                BlogPostsCount = dto.BlogPostsCount,
                EducationsCount = dto.EducationsCount,
                ExperiencesCount = dto.ExperiencesCount,
                CommentsCount = dto.CommentsCount,
                UsersCount = dto.UsersCount,
                ProjectsCount = dto.ProjectsCount,
            };

            return View(model);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(Index2));
        }
    }

    [AuthorizeRolesMvc("admin")]
    public IActionResult Index2()
    {
        return View();
    }
}
