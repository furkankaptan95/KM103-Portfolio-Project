using App.DTOs.AuthDtos;
using App.Services;
using App.ViewModels.AuthViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
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

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel registerModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerModel);
        }

        var dto = new RegisterDto
        {
            Email = registerModel.Email,
            Password = registerModel.Password,
            Username = registerModel.Username,
        };

        var result = await authService.RegisterAsync(dto);

        if (!result.IsSuccess)
        {
            ViewData["ErrorMessage"] = result.Message;
            return View(registerModel);
        }

        TempData["SuccessMessage"] = result.Message;

        return RedirectToAction(nameof(Login));
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string email, string token)
    {
        var dto = new VerifyEmailDto(email, token);

        var result = await authService.VerifyEmailAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
        }

        TempData["SuccessMessage"] = result.SuccessMessage;

        return RedirectToAction(nameof(Login));
    }
}
