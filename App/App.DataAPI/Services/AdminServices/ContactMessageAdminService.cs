﻿using App.Data.DbContexts;
using App.DTOs.ContactMessageDtos.Admin;
using App.Services;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.AdminServices;
public class ContactMessageAdminService(DataApiDbContext dataApiDb,IEmailService emailService) : IContactMessageAdminService
{
    public async Task<Result> ChangeIsReadAsync(int id)
    {
        try
        {
            var entity = await dataApiDb.ContactMessages.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.IsRead = true;

            dataApiDb.ContactMessages.Update(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result.Error("Bir hata oluştu: " + ex.Message);
        }
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

                if(entity.ReplyDate is not null)
                {
                    return Result<ContactMessageToReplyDto>.Conflict();
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
    
    public async Task<Result> ReplyContactMessageAsync(ReplyContactMessageDto dto)
    {
        try
        {
            var entity = await dataApiDb.ContactMessages.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            if(entity.ReplyDate is not null)
            {
                return Result.Conflict();
            }

            entity.IsRead = true;
            entity.Reply = dto.ReplyMessage;
            entity.ReplyDate = DateTime.Now;

            dataApiDb.ContactMessages.Update(entity);
            await dataApiDb.SaveChangesAsync();

            var htmlMailBody = $"<h1>Merhaba, Ben Furkan!</h1><p>{entity.Reply}</p>";
            var emailResult = await emailService.SendEmailAsync(entity.Email, "Benimle iletişime geçtiğiniz için teşekür ederim. İşte yanıtım!", htmlMailBody);

            if (emailResult.IsSuccess)
            {
                return Result.Success();
            }

            return Result.Error();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result.Error("Bir hata oluştu: " + ex.Message);
        }
    }
}
