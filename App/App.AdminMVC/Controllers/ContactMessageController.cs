using App.Core;
using App.Core.Authorization;
using App.DTOs.ContactMessageDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.ContactMessagesViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;

[AuthorizeRolesMvc("admin")]
public class ContactMessageController(IContactMessageAdminService contactMessageService) : Controller
{
    [HttpGet]
    [Route("all-contact-messages")]
    public async Task<IActionResult> AllContactMessages()
    {
        try
        {
            var result = await contactMessageService.GetAllContactMessagesAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/home/index");
            }

            var dtos = result.Value;

            List<AllContactMessagesViewModel> models = dtos
              .Select(item => new AllContactMessagesViewModel
              {
                  Id = item.Id,
                  ReplyDate = item.ReplyDate,
                  Name = item.Name,
                  Reply = item.Reply,
                  IsRead = item.IsRead,
                  Email = item.Email,
                  Message = item.Message,
                  SentDate = item.SentDate,
                  Subject = item.Subject,
              })
              .ToList();

            return View(models);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Mesajlar getirilirken beklenmedik bir hata oluştu..";
            return Redirect("/home/index");
        }
    }

    [HttpGet]
    [Route("reply-contact-message-{id:int}")]
    public async Task<IActionResult> ReplyContactMessage([FromRoute] int id)
    {
        try
        {
            var result = await contactMessageService.GetContactMessageByIdAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/all-contact-messages");
            }

            var dto = result.Value;

            var model = new ReplyViewModel();
            var modelGet = new GetContactMessageViewModel
            {
                Id = id,
                SentDate = dto.SentDate,
                Subject = dto.Subject,
                Name = dto.Name,
                Message = dto.Message,
                Email = dto.Email,
            };
           
            model.GetModel = modelGet;

            return View(model);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Mesaj verisi alınırken bir hata oluştu.";
            return Redirect("/all-contact-messages");
        }
    }

    [HttpPost]
    [Route("reply-message")]
    public async Task<IActionResult> ReplyContactMessage([FromForm] ReplyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new ReplyContactMessageDto
            {
                Id = model.ReplyModel.Id,
                ReplyMessage = model.ReplyModel.ReplyMessage,
            };

            var result = await contactMessageService.ReplyContactMessageAsync(dto);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault();

                if (result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = errorMessage;
                    return Redirect("/all-contact-messages");
                }

                ViewData["ErrorMessage"] = errorMessage;
                return View(model);
            }
            TempData["Message"] = result.SuccessMessage;
            return Redirect("/all-contact-messages");

        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Yanıt verme işlemi sırasında beklenmedik bir hata oluştu!..Tekrar deneyebilirsiniz.";
            return View(model);
        }

    }

    [HttpGet]
    [Route("make-message-read-{id:int}")]
    public async Task<IActionResult> MakeMessageRead([FromRoute] int id)
    {
        try
        {
            var result = await contactMessageService.ChangeIsReadAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }

            else
            {
                TempData["Message"] = result.SuccessMessage;
            }

            return Redirect("/all-contact-messages");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Mesaj okundu olarak işaretlenirken beklenmeyen bir hata oluştu..";
            return Redirect("/all-contact-messages");
        }
    }

}
