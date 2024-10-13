using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class AboutMeService(DataApiDbContext dataApiDb) : IAboutMeService
{
    public async Task<Result> AddAboutMeAsync(AddAboutMeApiDto dto)
    {
        try
        {
            var entity = new AboutMeEntity()
            {
                Introduction = dto.Introduction,
                ImageUrl1 = dto.ImageUrl1,
                ImageUrl2 = dto.ImageUrl2,
            };

            await dataApiDb.AboutMes.AddAsync(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.SuccessWithMessage(" -Hakkımda- bilgileri başarıyla eklendi. ");
        }
        catch (DbUpdateException dbEx)
        {
            return Result.Error("Veritabanı hatası: " + dbEx.Message);
        }
        catch (Exception ex)
        {
            return Result.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public Task<Result> AddAboutMeAsync(AddAboutMeMVCDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ShowAboutMeDto>> GetAboutMeAsync()
    {
        try
        {
            var entity = await dataApiDb.AboutMes.FirstOrDefaultAsync();

            if (entity == null)
            {
                return Result<ShowAboutMeDto>.NotFound("Hakkımda bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurunuz.");
            }

            var dto = new ShowAboutMeDto()
            {
                Introduction = entity.Introduction,
                ImageUrl1 = entity.ImageUrl1,
                ImageUrl2 = entity.ImageUrl2,
            };

            return Result<ShowAboutMeDto>.Success(dto);
        }

        catch (SqlException sqlEx)
        {
            return Result.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }

        catch (Exception ex)
        {
            var errorMessage = $"Bir hata oluştu: {ex.Message}, Hata Kodu: {ex.HResult}";
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result> UpdateAboutMeAsync(UpdateAboutMeApiDto dto)
    {
        try
        {
            var entity = await dataApiDb.AboutMes.FirstOrDefaultAsync();

            if (entity == null)
            {
                return Result.NotFound();
            }

            entity.Introduction = dto.Introduction;

            if (dto.ImageUrl1 != null) 
            {
                entity.ImageUrl1 = dto.ImageUrl1;
            }

            if (dto.ImageUrl2 != null)
            {
                entity.ImageUrl2 = dto.ImageUrl2;
            }

            dataApiDb.AboutMes.Update(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.SuccessWithMessage(" -Hakkında- bilgileriniz başarılı bir şekilde güncellendi. ");
        }

        catch (DbUpdateException dbEx)
        {
            return Result.Error("Veritabanı hatası: " + dbEx.Message);
        }

        catch (Exception ex)
        {
            var errorMessage = $"Bir hata oluştu: {ex.Message}, Hata Kodu: {ex.HResult}";
            return Result.Error(errorMessage);
        }
    }

    public Task<Result> UpdateAboutMeAsync(UpdateAboutMeMVCDto dto)
    {
        throw new NotImplementedException();
    }
}
