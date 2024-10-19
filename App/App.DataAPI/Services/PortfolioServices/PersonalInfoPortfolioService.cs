using App.Data.DbContexts;
using App.DTOs.PersonalInfoDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class PersonalInfoPortfolioService(DataApiDbContext dataApiDb) : IPersonalInfoPortfolioService
{
    public async Task<Result<PersonalInfoPortfolioDto>> GetPersonalInfoAsync()
    {
		try
		{
			var entity = await dataApiDb.PersonalInfos.FirstOrDefaultAsync();

			if (entity == null)
			{
				return Result<PersonalInfoPortfolioDto>.NotFound();
			}

			var dto = new PersonalInfoPortfolioDto()
			{
				Name = entity.Name,
				Surname = entity.Surname,
				About = entity.About,
				BirthDate = entity.BirthDate,
			};

			return Result<PersonalInfoPortfolioDto>.Success(dto);
		}
		catch (SqlException sqlEx)
		{
			return Result<PersonalInfoPortfolioDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
		}

		catch (Exception ex)
		{
			var errorMessage = $"Bir hata oluştu: {ex.Message}, Hata Kodu: {ex.HResult}";
			return Result<PersonalInfoPortfolioDto>.Error(errorMessage);
		}
	}
}
