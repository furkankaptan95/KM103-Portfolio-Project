using App.Core.Authorization;
using App.DTOs.UserDtos;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc.UserViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;

public class UserController(IUserPortfolioService userServive) : Controller
{
    [AuthorizeRolesMvc("admin", "commenter")]
    [HttpGet]
    public IActionResult MyProfile()
    {
        return View();
    }

    [AuthorizeRolesMvc("admin", "commenter")]
    [HttpGet]
    public async Task<IActionResult> EditUsername([FromForm] EditUsernameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new EditUsernameDto
        {
            Email = model.Email,
            Username = model.Username,
        };
        
        var result = await userServive.EditUsernameAsync(dto);

        if (!result.IsSuccess)
        {
            if(result.Status == ResultStatus.NotFound)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/");
            }

            ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return View(model);
        }

        TempData["SuccessMessage"] = result.SuccessMessage;

        return RedirectToAction(nameof(MyProfile));
        
    }
}
