using App.Data.DbContexts;
using App.DTOs.AboutMeDtos.Porfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class AboutMePortfolioService(DataApiDbContext dataApiDb) : IAboutMePortfolioService
{
    public async Task<Result<AboutMePortfolioDto>> GetAboutMeAsync()
    {
        try
        {
            var entity = await dataApiDb.AboutMes.FirstOrDefaultAsync();

            if (entity == null)
            {
                return Result<AboutMePortfolioDto>.NotFound();
            }

            var dto = new AboutMePortfolioDto()
            {
                Field =entity.Field,
                FullName =entity.FullName,
                Introduction = entity.Introduction,
                ImageUrl1 = entity.ImageUrl1,
                ImageUrl2 = entity.ImageUrl2,
            };

            return Result<AboutMePortfolioDto>.Success(dto);
        }

        catch (SqlException sqlEx)
        {
            return Result<AboutMePortfolioDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }

        catch (Exception ex)
        {
            var errorMessage = $"Bir hata oluştu: {ex.Message}, Hata Kodu: {ex.HResult}";
            return Result<AboutMePortfolioDto>.Error(errorMessage);
        }
    }
}
