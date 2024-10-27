using App.Core.Authorization;
using App.DTOs.UserDtos;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc.UserViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
    [HttpPost]
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

            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return RedirectToAction(nameof(MyProfile));
        }

        var tokens = result.Value;

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

        TempData["SuccessMessage"] = result.SuccessMessage;

        return RedirectToAction(nameof(MyProfile));

    }

    [AuthorizeRolesMvc("admin", "commenter")]
    [HttpPost]
    public async Task<IActionResult> EditUserImage([FromForm] EditUserImageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(MyProfile));
        }

        var dto = new EditUserImageMvcDto
        {
            Email = model.Email,
            ImageFile = model.ImageFile,
        };

        var result = await userServive.ChangeUserImageAsync(dto);

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

        TempData["SuccessMessage"] = result.SuccessMessage;

        return RedirectToAction(nameof(MyProfile));

    }

    [AuthorizeRolesMvc("admin", "commenter")]
    [HttpGet("delete-user-img-{userImageUrl}")]
    public async Task<IActionResult> DeleteUserImage([FromRoute] string userImageUrl)
    {
        
        var result = await userServive.DeleteUserImageAsync(userImageUrl);

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

        TempData["SuccessMessage"] = result.SuccessMessage;

        return RedirectToAction(nameof(MyProfile));

    }
}
