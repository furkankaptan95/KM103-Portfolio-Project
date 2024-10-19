using App.Data.DbContexts;
using App.DTOs.BlogPostDtos.Porfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class BlogPosPortfolioService(DataApiDbContext dataApiDb,ICommentPortfolioService commentPortfolioService) : IBlogPosPortfolioService
{
	public Task<Result<List<AllBlogPostsPortfolioDto>>> GetAllBlogPostsAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Result<SingleBlogPostDto>> GetBlogPostById(int id)
	{
		try
		{
			var entity = await dataApiDb.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

			if (entity is null)
			{
				return Result<SingleBlogPostDto>.NotFound();
			}

			var dto = new SingleBlogPostDto
			{
				Id = id,
				Title = entity.Title,
				Content = entity.Content,
				PublishDate = entity.PublishDate,

			};

			return Result<SingleBlogPostDto>.Success(dto);
		}
		catch (SqlException sqlEx)
		{
			return Result<SingleBlogPostDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
		}
		catch (Exception ex)
		{
			return Result<SingleBlogPostDto>.Error("Bir hata oluştu: " + ex.Message);
		}
	}
}
