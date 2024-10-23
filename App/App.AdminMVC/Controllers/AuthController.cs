using App.DTOs.AuthDtos;
using App.Services;
using App.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class AuthController(IAuthService authService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new LoginDto(model.Email, model.Password);

        var result = await authService.LoginAsync(dto);

        if (!result.IsSuccess)
        {
            return View(model);
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

        HttpContext.Response.Cookies.Append("AccessToken", tokens.AccessToken, jwtCookieOptions);
        HttpContext.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

        return Redirect("/");
    }
}
