using App.DTOs.ContactMessageDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services.AdminServices;
public class ContactMessageAdminService : IContactMessageAdminService
{
    public Task<Result> ChangeIsReadAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllContactMessagesDto>>> GetAllContactMessagesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> ReplyContactMessageAsync(ReplyContactMessageDto dto)
    {
        throw new NotImplementedException();
    }
}
