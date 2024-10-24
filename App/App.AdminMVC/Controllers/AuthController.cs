using App.DTOs.AuthDtos;
using App.Services.AuthService.Abstract;
using App.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        HttpContext.Response.Cookies.Append("AccessToken", tokens.JwtToken, jwtCookieOptions);
        HttpContext.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

        return Redirect("/");
    }

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        var refreshToken = Request.Cookies["RefreshToken"];

        if (refreshToken.IsNullOrEmpty())
        {
            ViewData["SuccessMessage"] = "Hesabınızdan başarıyla çıkış yapıldı.";
            return View(nameof(Login));
        }

        var result = await authService.RevokeTokenAsync(refreshToken);

        if (result.IsSuccess)
        {
            Response.Cookies.Delete("JwtToken");
            Response.Cookies.Delete("RefreshToken");

            ViewData["SuccessMessage"] = result.SuccessMessage;
            return View(nameof(Login));
        }

        TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        return Redirect("/");
    }
}
