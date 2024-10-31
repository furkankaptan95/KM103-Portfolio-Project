using App.DTOs.ContactMessageDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class ContactMessageService(IHttpClientFactory factory) : IContactMessageAdminService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> ChangeIsReadAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"make-message-read-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Mesaj okundu olarak işaretlendi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Okundu olarak işaretlemek istediğiniz Mesaj bulunamadı!..";
            }
            else
            {
                errorMessage = "Mesaj okundu olarak işaretlenirken beklenmeyen bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }

        catch (Exception)
        {
            return Result.Error("Mesaj okundu olarak işaretlenirken beklenmeyen bir hata oluştu..");
        }
    }

    public async Task<Result<List<AllContactMessagesDto>>> GetAllContactMessagesAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("all-contact-messages");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllContactMessagesDto>>.Error("Mesajlar getirilirken beklenmedik bir hata oluştu..");
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllContactMessagesDto>>>();

            if (result is null)
            {
                return Result<List<AllContactMessagesDto>>.Error("Mesajlar getirilirken beklenmedik bir hata oluştu..");
            }

            return result;
        }

        catch (Exception)
        {
            return Result<List<AllContactMessagesDto>>.Error("Mesajlar getirilirken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result<ContactMessageToReplyDto>> GetContactMessageByIdAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"get-contact-message-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<ContactMessageToReplyDto>>();

                if (result is null)
                {
                    return Result<ContactMessageToReplyDto>.Error("Mesaj verisi alınırken bir hata oluştu.");
                }

                return result;
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Yanıtlamak istediğiniz Mesaj bulunamadı!..";
            }
            else if(apiResponse.StatusCode == HttpStatusCode.Conflict)
            {
                errorMessage = "Mesajı daha önce zaten yanıtladınız!..";
            }
            else
            {
                errorMessage = "Mesaj verisi alınırken bir hata oluştu.";
            }

            return Result<ContactMessageToReplyDto>.Error(errorMessage);
        }

        catch (Exception)
        {
            return Result<ContactMessageToReplyDto>.Error("Mesaj verisi alınırken beklenmedik bir hata oluştu.");
        }
    }

    public async Task<Result> ReplyContactMessageAsync(ReplyContactMessageDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PutAsJsonAsync("reply-contact-message", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Error("Yanıt vermek istediğiniz Mesaj bulunamadı.");
                }
                else if (apiResponse.StatusCode == HttpStatusCode.Conflict)
                {
                    return Result.Error("Mesajı daha önce zaten yanıtladınız!..");
                }

                return Result.Error("Yanıt verme işlemi sırasında beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage("Yanıt başarıyla gönderildi.");
        }
        catch (Exception)
        {
            return Result.Error("Yanıt verme işlemi sırasında beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }
}
