using App.Core.Authorization;
using App.DTOs.ContactMessageDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
[AllowAnonymousManuel]
public class ContactMessageController(IContactMessagePortfolioService contactMessageService) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([FromForm] AddContactMessageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect("/#contact-section");
        }

        try
        {
            var dto = new AddContactMessageDto
            {
                Email = model.Email,
                Subject = model.Subject,
                Name = model.Name,
                Message = model.Message,
            };

            var result = await contactMessageService.AddContactMessageAsync(dto);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/#contact-section");
            }

            TempData["SuccessMessage"] = result.SuccessMessage;
            return Redirect("/#contact-section");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "İletişim Formu gönderilirken bir problem oluştu!..";
            return Redirect("/#contact-section");
        }

    }
}
