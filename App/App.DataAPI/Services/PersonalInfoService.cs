using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class PersonalInfoService(DataApiDbContext dataApiDb) : IPersonalInfoService
{
    public async Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto)
    {
        try
        {
            var entity = new PersonalInfoEntity()
            {
                About = dto.About,
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
            };

            await dataApiDb.PersonalInfos.AddAsync(entity);
            await dataApiDb.SaveChangesAsync();

            return Result.Success();
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

    public async Task<Result<ShowPersonalInfoDto>> GetPersonalInfoAsync()
    {
        try
        {
            var entity = await dataApiDb.PersonalInfos.FirstOrDefaultAsync();

            if (entity == null)
            {
                return Result<ShowPersonalInfoDto>.NotFound();
            }

            var dto = new ShowPersonalInfoDto()
            {
                Name = entity.Name,
                Surname = entity.Surname,
                About = entity.About,
                BirthDate = entity.BirthDate,
            };

            return Result<ShowPersonalInfoDto>.Success(dto);
        }

        catch (SqlException sqlEx)
        {
            return Result<ShowPersonalInfoDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }

        catch (Exception ex)
        {
            var errorMessage = $"Bir hata oluştu: {ex.Message}, Hata Kodu: {ex.HResult}";
            return Result<ShowPersonalInfoDto>.Error(errorMessage);
        }
    }

    public Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto)
    {
        throw new NotImplementedException();
    }
}
