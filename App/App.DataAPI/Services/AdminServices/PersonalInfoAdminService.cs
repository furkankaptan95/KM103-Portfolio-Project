﻿using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.PersonalInfoDtos;
using App.DTOs.PersonalInfoDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.AdminServices;
public class PersonalInfoAdminService(DataApiDbContext dataApiDb) : IPersonalInfoAdminService
{
    public async Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto)
    {
        try
        {
            var entityCheck = await dataApiDb.PersonalInfos.FirstOrDefaultAsync();

            if(entityCheck is not null)
            {
                return Result.Conflict();
            }

            var entity = new PersonalInfoEntity()
            {
                About = dto.About,
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                Link = dto.Link,
                Adress = dto.Adress,
            };

            await dataApiDb.PersonalInfos.AddAsync(entity);
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

    public async Task<Result<bool>> CheckPersonalInfoAsync()
    {
        try
        {
            var entity = await dataApiDb.PersonalInfos.FirstOrDefaultAsync();

            if (entity == null)
            {
                return Result.Success(false);
            }

            return Result.Success(true);

        }

        catch (SqlException sqlEx)
        {
            return Result<bool>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }

        catch (Exception ex)
        {
            var errorMessage = $"Bir hata oluştu: {ex.Message}, Hata Kodu: {ex.HResult}";
            return Result<bool>.Error(errorMessage);
        }
    }

    public async Task<Result<PersonalInfoAdminDto>> GetPersonalInfoAsync()
    {
        try
        {
            var entity = await dataApiDb.PersonalInfos.FirstOrDefaultAsync();

            if (entity == null)
            {
                return Result<PersonalInfoAdminDto>.NotFound();
            }

            var dto = new PersonalInfoAdminDto()
            {
                Name = entity.Name,
                Surname = entity.Surname,
                About = entity.About,
                BirthDate = entity.BirthDate,
                Link = entity.Link,
                Adress = entity.Adress,
                Email = entity.Email,
            };

            return Result<PersonalInfoAdminDto>.Success(dto);
        }
        catch (SqlException sqlEx)
        {
            return Result<PersonalInfoAdminDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }

        catch (Exception ex)
        {
            var errorMessage = $"Bir hata oluştu: {ex.Message}, Hata Kodu: {ex.HResult}";
            return Result<PersonalInfoAdminDto>.Error(errorMessage);
        }
    }

    public async Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto)
    {
        try
        {
            var entity = await dataApiDb.PersonalInfos.FirstOrDefaultAsync();

            if (entity == null)
            {
                return Result.NotFound();
            }

            entity.Name = dto.Name;
            entity.Surname = dto.Surname;
            entity.About = dto.About;
            entity.BirthDate = dto.BirthDate;
            entity.Link = dto.Link;
            entity.Adress = dto.Adress;
            entity.Email = dto.Email;

            dataApiDb.PersonalInfos.Update(entity);
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
            var errorMessage = $"Bir hata oluştu: {ex.Message}, Hata Kodu: {ex.HResult}";
            return Result.Error(errorMessage);
        }
    }
}
