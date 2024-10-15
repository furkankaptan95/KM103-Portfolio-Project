using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;

public class HomeController(IHomeService homeService) : Controller
{
    public async Task<IActionResult> Index()
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

    public async Task<IActionResult> Index2()
    {
        return View();
    }

}
