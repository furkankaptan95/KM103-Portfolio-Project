using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.ContactMessageDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class ContactMessagePortfolioService(DataApiDbContext dataApiDb) : IContactMessagePortfolioService
{
    public async Task<Result> AddContactMessageAsync(AddContactMessageDto dto)
    {
        try
        {
            var entity = new ContactMessageEntity()
            {
                Name = dto.Name,
                Email = dto.Email,
                Subject = dto.Subject,
                Message = dto.Message,
            };

            await dataApiDb.ContactMessages.AddAsync(entity);
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
}
