using App.DTOs.ContactMessageDtos.Admin;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IContactMessageService
{
    Task<Result<List<AllContactMessagesDto>>> GetAllContactMessagesAsync();
    Task<Result> ReplyContactMessageAsync(ReplyContactMessageDto dto);
    Task<Result> ChangeIsReadAsync(int id);
}
