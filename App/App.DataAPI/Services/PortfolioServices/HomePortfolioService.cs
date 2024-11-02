using App.Data.DbContexts;
using App.DTOs.ExperienceDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class HomePortfolioService(DataApiDbContext dataApiDb) : IHomePortfolioService
{
	public Task<Result<byte[]>> DownloadCvAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Result<string>> GetCvUrlAsync()
	{
		try
		{
			var cvEntity = await dataApiDb.CVs.FirstOrDefaultAsync();

			if(cvEntity is not null)
			{
				return Result<string>.Success(cvEntity.Url);
			}

			return Result<string>.Error();
		}
		catch (SqlException sqlEx)
		{
			return Result<string>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
		}
		catch (Exception ex)
		{
			return Result<string>.Error("Bir hata oluştu: " + ex.Message);
		}
	}

	public Task<Result<HomeIndexViewModel>> GetHomeInfosAsync()
	{
		throw new NotImplementedException();
	}
}
