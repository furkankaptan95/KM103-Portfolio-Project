using App.Data.DbContexts;
using App.Services.PortfolioServices.Abstract;
using App.DTOs.ExperienceDtos.Portfolio;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class ExperiencePortfolioService(DataApiDbContext dataApiDb) : IExperiencePortfolioService
{
	public async Task<Result<List<AllExperiencesPortfolioDto>>> GetAllExperiencesAsync()
	{
		try
		{
			var dtos = new List<AllExperiencesPortfolioDto>();

			var entities = await dataApiDb.Experiences.Where(bp => bp.IsVisible == true).ToListAsync();

			if (entities is null)
			{
				return Result<List<AllExperiencesPortfolioDto>>.Success(dtos);
			}

			dtos = entities
		   .Select(item => new AllExperiencesPortfolioDto
		   {
			   Company = item.Company,
			   Description = item.Description,
			   StartDate = item.StartDate,
			   EndDate = item.EndDate,
			   Title = item.Title,
		   })
		   .ToList();

			return Result<List<AllExperiencesPortfolioDto>>.Success(dtos);
		}
		catch (SqlException sqlEx)
		{
			return Result<List<AllExperiencesPortfolioDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
		}
		catch (Exception ex)
		{
			return Result<List<AllExperiencesPortfolioDto>>.Error("Bir hata oluştu: " + ex.Message);
		}
	}
}
