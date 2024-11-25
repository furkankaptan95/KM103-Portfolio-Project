using App.DTOs.AuthDtos;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.CommentsViewModels;
using App.ViewModels.AdminMvc.UsersViewModels;
using App.ViewModels.PortfolioMvc.UserViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.AdminMVC.Controllers;

[Authorize(Roles = "admin")]
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
                return Redirect("/");
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
               ImageUrl = item.ImageUrl ?? "default.png",
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
            return Redirect("/");
        }
    }

    [HttpGet]
    [Route("change-user-activeness-{id:int}")]
    public async Task<IActionResult> ChangeUserActiveness([FromRoute] int id)
    {
        if (id < 1)
        {

            TempData["ErrorMessage"] = "Geçersiz Kullanıcı ID Bilgisi!..";
            return Redirect("/all-users");
        }
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

    [HttpGet]
    public IActionResult MyProfile()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUsername([FromForm] EditUsernameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(MyProfile));
        }

        if (model.Username == User.FindFirst("name")?.Value)
        {
            return RedirectToAction(nameof(MyProfile));
        }

        try
        {
            var dto = new EditUsernameDto
            {
                Email = model.Email,
                Username = model.Username,
            };

            var result = await userService.EditUsernameAsync(dto);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                    return Redirect("/");
                }

                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return RedirectToAction(nameof(MyProfile));
            }

            var tokens = result.Value;

            SetCookies(tokens);

            TempData["SuccessMessage"] = result.SuccessMessage;

            return RedirectToAction(nameof(MyProfile));
        }

        catch (Exception)
        {
            TempData["ErrorMessage"] = "Kullanıcı ismi değiştirilirken bir hata oluştu!..";
            return RedirectToAction(nameof(MyProfile));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUserImage([FromForm] EditUserImageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(MyProfile));
        }

        try
        {
            var dto = new EditUserImageMvcDto
            {
                Email = model.Email,
                ImageFile = model.ImageFile,
            };

            var result = await userService.ChangeUserImageAsync(dto);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                    return Redirect("/");
                }

                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return RedirectToAction(nameof(MyProfile));
            }

            var tokens = result.Value;

            SetCookies(tokens);

            TempData["SuccessMessage"] = result.SuccessMessage;

            return RedirectToAction(nameof(MyProfile));
        }

        catch (Exception)
        {
            TempData["ErrorMessage"] = "Profil resmi güncellenirken bir hata oluştu!..";

            return RedirectToAction(nameof(MyProfile));
        }
    }

    [HttpGet("delete-user-img-{userImageUrl}")]
    public async Task<IActionResult> DeleteUserImage([FromRoute] string userImageUrl)
    {
        if (string.IsNullOrEmpty(userImageUrl))
        {
            return RedirectToAction(nameof(MyProfile));
        }

        try
        {
            var result = await userService.DeleteUserImageAsync(userImageUrl);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                    return Redirect("/");
                }

                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return RedirectToAction(nameof(MyProfile));
            }

            var tokens = result.Value;

            SetCookies(tokens);

            TempData["SuccessMessage"] = result.SuccessMessage;

            return RedirectToAction(nameof(MyProfile));
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Profil resminiz silinirken bir hata oluştu!..";

            return RedirectToAction(nameof(MyProfile));
        }
    }

    private void SetCookies(TokensDto tokens)
    {
        CookieOptions jwtCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddMinutes(10) // JWT ile aynı süre
        };

        // Refresh token için de süre ayarlanabilir
        CookieOptions refreshTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(7) // Refresh token süresi
        };

        HttpContext.Response.Cookies.Append("JwtToken", tokens.JwtToken, jwtCookieOptions);
        HttpContext.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

        // JWT'den ClaimsPrincipal oluştur
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(tokens.JwtToken) as JwtSecurityToken;
        var identity = new ClaimsIdentity(jwtToken?.Claims, "jwt"); // veya "Bearer"
        HttpContext.User = new ClaimsPrincipal(identity); // Kullanıcı bilgilerini ayarla
    }
}
