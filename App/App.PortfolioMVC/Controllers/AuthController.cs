﻿using App.DTOs.AuthDtos;
using App.Services;
using App.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class AuthController(IAuthService authService) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
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

        return Redirect("/");
    }
}