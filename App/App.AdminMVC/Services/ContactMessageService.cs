namespace App.AdminMVC.Services;

using App.DTOs.BlogPostDtos.Admin;
using App.DTOs.ContactMessageDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

public class ContactMessageService(IHttpClientFactory factory) : IContactMessageAdminService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> ChangeIsReadAsync(int id)
    {
        throw new NotImplementedException();
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
                errorMessage = "Düzenlemek istediğiniz Mesaj bulunamadı!..";
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

    public Task<Result> ReplyContactMessageAsync(ReplyContactMessageDto dto)
    {
        throw new NotImplementedException();
    }
}
