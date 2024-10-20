using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.ContactMessagesViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
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
    public async Task<IActionResult> ReplyMessage([FromRoute] int id)
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

            var model = new ReplyContactMessageViewModel()
            {
                MessageId = id,
                SentDate = dto.SentDate,
                Subject = dto.Subject,
                Name = dto.Name,
                Message = dto.Message,
                Email = dto.Email,
            };

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
    public async Task<IActionResult> ReplyContactMessage([FromForm] ReplyContactMessageViewModel model)
    {
        return View(model);
    }

}
