using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services
{
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

                return Result.Success();
            }
            catch (DbUpdateException dbEx)
            {
                // Veritabanı ile ilgili bir hata oluştu
                return Result.Error("Veritabanı hatası: " + dbEx.Message);
            }
            catch (Exception ex)
            {
                // Diğer tüm hatalar
                return Result.Error("Bir hata oluştu: " + ex.Message);
            }
        }

        public Task<Result> AddAboutMeAsync(AddAboutMeMVCDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<ShowAboutMeDto>> GetAboutMeAsync()
        {
            var entity = await dataApiDb.AboutMes.FirstOrDefaultAsync();

            if (entity == null)
            {
                return Result<ShowAboutMeDto>.NotFound();
            }

            var dto = new ShowAboutMeDto()
            {
                Introduction = entity.Introduction,
                ImageUrl1 = entity.ImageUrl1,
                ImageUrl2 = entity.ImageUrl2,
            };

            return Result<ShowAboutMeDto>.Success(dto);

        }

        public Task<Result> UpdateAboutMeAsync(UpdateAboutMeApiDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAboutMeAsync(UpdateAboutMeMVCDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
