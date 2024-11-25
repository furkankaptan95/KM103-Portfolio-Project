using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.HomeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;

public class HomeController(IHomeAdminService homeService) : Controller
{

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
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

    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult Index2()
    {
        return View();
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult UploadCV()
    {
        return View();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> UploadCV([FromForm] IFormFile cvFile)
    {

        if (cvFile == null || cvFile.Length == 0)
        {
            ViewData["ErrorMessage"] = "L�tfen bir dosya se�in.";
            return View();
        }

        // Dosyan�n PDF olup olmad���n� kontrol et
        if (cvFile.ContentType != "application/pdf")
        {
            ViewData["ErrorMessage"] = "Sadece PDF format�ndaki dosyalar� y�kleyebilirsiniz.";
            return View();
        }

        // Alternatif olarak, dosya ad�n�n .pdf uzant�s� ile bitti�ini de kontrol edebilirsiniz:
        if (!cvFile.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            ViewData["ErrorMessage"] = "Dosya format� yaln�zca PDF olabilir.";
            return View();
        }

        try
        {
            var result = await homeService.UploadCvAsync(cvFile);

            if (result.IsSuccess)
            {
                ViewData["Message"] = result.SuccessMessage;
                return View();
            }

            ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return View();

        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "CV y�klenirken beklenmeyen bir hata olu�tu.Tekrar deneyebilirsiniz.";
            return View();
        }
    }
}
