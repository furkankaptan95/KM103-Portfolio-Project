using App.Data.DbContexts;
using App.DTOs.BlogPostDtos.Admin;
using App.DTOs.ContactMessageDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.AdminServices;
public class ContactMessageAdminService(DataApiDbContext dataApiDb) : IContactMessageAdminService
{
    public Task<Result> ChangeIsReadAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllContactMessagesDto>>> GetAllContactMessagesAsync()
    {
        try
        {
            var dtos = new List<AllContactMessagesDto>();

            var entities = await dataApiDb.ContactMessages.ToListAsync();

            if (entities is null)
            {
                return Result<List<AllContactMessagesDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new AllContactMessagesDto
           {
               Id = item.Id,
               Name = item.Name,
               Email = item.Email,
               Subject = item.Subject,
               Message = item.Message,
               SentDate = item.SentDate,
               IsRead = item.IsRead,
               Reply = item.Reply,
               ReplyDate = item.ReplyDate,
           })
           .ToList();

            return Result<List<AllContactMessagesDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllContactMessagesDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllContactMessagesDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result<ContactMessageToReplyDto>> GetContactMessageByIdAsync(int id)
    {
            try
            {
                var entity = await dataApiDb.ContactMessages.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is null)
                {
                    return Result<ContactMessageToReplyDto>.NotFound();
                }

                var dto = new ContactMessageToReplyDto
                {
                    Id = id,
                    Email = entity.Email,
                    Subject = entity.Subject,
                    Message = entity.Message,
                    SentDate = entity.SentDate,
                    Name = entity.Name,
                };

                return Result<ContactMessageToReplyDto>.Success(dto);
            }
            catch (SqlException sqlEx)
            {
                return Result<ContactMessageToReplyDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                return Result<ContactMessageToReplyDto>.Error("Bir hata oluştu: " + ex.Message);
            }
        }
    
    public Task<Result> ReplyContactMessageAsync(ReplyContactMessageDto dto)
    {
        throw new NotImplementedException();
    }
}
