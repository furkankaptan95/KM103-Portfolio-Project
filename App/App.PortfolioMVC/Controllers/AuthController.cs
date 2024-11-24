﻿using App.DTOs.AuthDtos;
using App.Services.AuthService.Abstract;
using App.ViewModels.AuthViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.PortfolioMVC.Controllers;

[AllowAnonymous]
public class AuthController(IAuthService authService) : Controller
{
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {

            var dto = new LoginDto(model.Email, model.Password,false);

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
                Expires = DateTime.UtcNow.AddMinutes(10)
            };

            CookieOptions refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            HttpContext.Response.Cookies.Append("JwtToken", tokens.JwtToken, jwtCookieOptions);
            HttpContext.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshTokenCookieOptions);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(tokens.JwtToken) as JwtSecurityToken;
            var identity = new ClaimsIdentity(jwtToken?.Claims, "jwt");
            HttpContext.User = new ClaimsPrincipal(identity);

            TempData["SuccessMessage"] = result.SuccessMessage;
            return Redirect("/");
        }

        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Giriş işlemi sırasında bir hata oluştu!..";
            return View(model);
        }
    }
 
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel registerModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerModel);
        }

        try
        {
            var request = HttpContext.Request;
            string url = $"{request.Scheme}://{request.Host}";

            var dto = new RegisterDto
            {
                Email = registerModel.Email,
                Password = registerModel.Password,
                Username = registerModel.Username,
                Url = url,
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

        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Kayıt işlemi sırasında beklenmeyen bir hata oluştu!..";
            return View(registerModel);
        }
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string email, string token)
    {
        if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            TempData["ErrorMessage"] = "Email doğrulama başarısız!..Tekrar doğrulama maili almak için mail adresinizi girebilirsiniz.";
            return RedirectToAction(nameof(NewVerificationMail));
        }

        try
        {
            var dto = new VerifyEmailDto(email, token);

            var result = await authService.VerifyEmailAsync(dto);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return RedirectToAction(nameof(NewVerificationMail));
            }

            TempData["SuccessMessage"] = result.SuccessMessage;

            return RedirectToAction(nameof(Login));
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Email doğrulama başarısız!..Tekrar doğrulama maili almak için mail adresinizi girebilirsiniz.";
            return RedirectToAction(nameof(NewVerificationMail));
        }
    }

    [HttpGet]
    public IActionResult NewVerificationMail()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewVerificationMail([FromForm] NewVerificationMailViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = HttpContext.Request;
            string url = $"{request.Scheme}://{request.Host}";

            var dto = new NewVerificationMailDto
            {
                Email = model.Email,
                Url = url,
            };

            var result = await authService.NewVerificationAsync(dto);

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
            ViewData["ErrorMessage"] = "Aktivasyon linki gönderilirken bir hata oluştu!..";

            return View(model);
        }
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

            var dto = new ForgotPasswordDto(model.Email, url,false);

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
        if (string.IsNullOrEmpty(token))
        {
            TempData["ErrorMessage"] = "Email adresiniz doğrulanamadı. Tekrar deneyebilirsiniz.";
            return RedirectToAction(nameof(ForgotPassword));
        }
        try
        {
            var dto = new RenewPasswordDto(email, token,false);

            var result = await authService.RenewPasswordEmailAsync(dto);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return RedirectToAction(nameof(ForgotPassword));
            }

            ViewData["SuccessMessage"] = result.SuccessMessage;

            var model = new NewPasswordViewModel
            {
                Email = email
            };

            return View(model);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Email adresiniz doğrulanırken bir problem oluştu!..";
            return RedirectToAction(nameof(ForgotPassword));
        }
    }


    [HttpPost("renew-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RenewPassword([FromForm] NewPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
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
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Şifreniz sıfırlanırken bir hata oluştu..Tekrar sıfırlama maili gönderebilirsiniz.";
            return RedirectToAction(nameof(ForgotPassword));
        }
    }


    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        try
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                Response.Cookies.Delete("JwtToken");

                TempData["SuccessMessage"] = "Hesabınızdan başarıyla çıkış yapıldı.";
                return Redirect("/");

            }

            var result = await authService.RevokeTokenAsync(refreshToken);

            if (result.IsSuccess || result.Status == ResultStatus.NotFound)
            {
                Response.Cookies.Delete("JwtToken");
                Response.Cookies.Delete("RefreshToken");

                TempData["SuccessMessage"] = result.SuccessMessage;
                return Redirect("/");
            }

            TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return Redirect("/");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Hesabınızdan çıkış yapılırken bir hata oluştu!..";
            return Redirect("/");
        }
    }
}
