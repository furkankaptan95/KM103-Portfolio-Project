using App.Core;
using App.DTOs.AuthDtos;
using App.Services.AuthService.Abstract;
using App.ViewModels.AuthViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace App.AdminMVC.Controllers;
[AllowAnonymousManuel]
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

        try
        {

            var dto = new LoginDto(model.Email, model.Password);

            var result = await authService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                string errorMessage = result.Errors.FirstOrDefault();

                if (result.Status == ResultStatus.Forbidden)
                {
                    ViewData["ErrorMessage"] = errorMessage;
                    return View();
                }

                ViewData["ErrorMessage"] = errorMessage;

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

            HttpContext.Response.Cookies.Append("JwtToken", tokens.JwtToken, jwtCookieOptions);
            HttpContext.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

            TempData["SuccessMessage"] = result.SuccessMessage;
            return Redirect("/");
        }

        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Giriş işlemi sırasında bir hata oluştu!..";

            return View(model);
        }
    }

    public async Task<IActionResult> ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = HttpContext.Request;
            string url = $"{request.Scheme}://{request.Host}";

            var dto = new ForgotPasswordDto(model.Email, url);

            var result = await authService.ForgotPasswordAsync(dto);

            if (!result.IsSuccess)
            {
                ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return View(model);
            }

            ViewData["SuccessMessage"] = result.SuccessMessage;

            return View();
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Şifre sıfırlama linki gönderilirken bir hata oluştu!..";

            return View(model);
        }

    }

    [HttpGet("renew-password")]
    public async Task<IActionResult> RenewPassword([FromQuery] string email, string token)
    {
        //null token kontrolü

        var dto = new RenewPasswordDto(email, token);

        var result = await authService.RenewPasswordEmailAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/");
        }

        ViewData["SuccessMessage"] = result.SuccessMessage;

        var model = new NewPasswordViewModel
        {
            Email = email
        };
        return View(model);
    }

    [HttpPost("renew-password")]
    public async Task<IActionResult> RenewPassword([FromForm] NewPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new NewPasswordDto() { Email = model.Email, Password = model.Password };

        var result = await authService.NewPasswordAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return RedirectToAction(nameof(ForgotPassword));
        }

        TempData["SuccessMessage"] = result.SuccessMessage;
        return RedirectToAction(nameof(Login));
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
