using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
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

    public Task<Result<ShowPersonalInfoDto>> GetPersonalInfoAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto)
    {
        throw new NotImplementedException();
    }
}
