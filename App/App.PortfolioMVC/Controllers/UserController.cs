using App.Core.Authorization;
using App.DTOs.AuthDtos;
using App.DTOs.UserDtos;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc.UserViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.PortfolioMVC.Controllers;

[AuthorizeRolesMvc("commenter")]
public class UserController(IUserPortfolioService userService) : Controller
{
    
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

        if(model.Username == User.FindFirst("name")?.Value)
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
