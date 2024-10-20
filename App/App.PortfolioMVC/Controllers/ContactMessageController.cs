using App.DTOs.ContactMessageDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;
public class ContactMessageController(IContactMessagePortfolioService contactMessageService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] AddContactMessageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect("/#contact-section");
        }

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

        TempData["Message"] = result.SuccessMessage;
        return Redirect("/#contact-section");
    }
}
