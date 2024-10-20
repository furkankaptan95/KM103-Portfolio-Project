namespace App.AdminMVC.Services;

using App.DTOs.ContactMessageDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ContactMessageService : IContactMessageAdminService
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
