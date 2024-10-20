namespace App.AdminMVC.Services;

using App.DTOs.ContactMessageDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Collections.Generic;
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

    public Task<Result<ContactMessageToReplyDto>> GetContactMessageByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ReplyContactMessageAsync(ReplyContactMessageDto dto)
    {
        throw new NotImplementedException();
    }
}
