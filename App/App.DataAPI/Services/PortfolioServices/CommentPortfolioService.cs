using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.CommentDtos.Admin;
using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class CommentPortfolioService : ICommentPortfolioService
{
	private readonly DataApiDbContext _dataApiDb;
	private readonly IHttpClientFactory _factory;

	public CommentPortfolioService(DataApiDbContext dataApiDb, IHttpClientFactory factory)
	{
		_dataApiDb = dataApiDb;
		_factory = factory;
	}
	private HttpClient AuthApiClient => _factory.CreateClient("authApi");
	public async Task<Result> AddCommentAsync(AddCommentDto dto)
    {
        try
        {
            var entity = new CommentEntity()
            {
              Content = dto.Content,
			  BlogPostId = dto.BlogPostId,
			  UnsignedCommenterName = dto.UnsignedCommenterName,
			  UserId = dto.UserId,
            };

            await _dataApiDb.Comments.AddAsync(entity);
            await _dataApiDb.SaveChangesAsync();

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

	public async Task<Result<List<BlogPostCommentsPortfolioDto>>> GetBlogPostCommentsAsync(int id)
	{
		try
		{
			var dtos = new List<BlogPostCommentsPortfolioDto>();

			var entities = await _dataApiDb.Comments.Where(c=>c.BlogPostId == id && c.IsApproved==true).ToListAsync();

			if (entities is null)
			{
				return Result<List<BlogPostCommentsPortfolioDto>>.Success(dtos);
			}

			foreach (var item in entities)
			{
				string commenter;

				if (item.UnsignedCommenterName != null)
				{
					commenter = item.UnsignedCommenterName;
				}
				else
				{
					if (item.UserId.HasValue)
					{
						var authApiResponse = await AuthApiClient.GetAsync($"get-commenter-username-{item.UserId}");

						if (!authApiResponse.IsSuccessStatusCode)
						{
							commenter = "Unknown User";
						}

						else
						{
							var result = await authApiResponse.Content.ReadFromJsonAsync<Result<string>>();

							if (result is null)
							{
								commenter = "Unknown User";
							}
							else
							{
								commenter = result.Value;
							}
						}
					}
					else
					{
						commenter = "Anonymous";
					}
				}

				var dto = new BlogPostCommentsPortfolioDto
				{
					Id = item.Id,
					Content = item.Content,
					CreatedAt = item.CreatedAt,
					Commenter = commenter
				};

				dtos.Add(dto);
			}

			return Result<List<BlogPostCommentsPortfolioDto>>.Success(dtos);
		}
		catch (SqlException sqlEx)
		{
			return Result<List<BlogPostCommentsPortfolioDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
		}
		catch (Exception ex)
		{
			return Result<List<BlogPostCommentsPortfolioDto>>.Error("Bir hata oluştu: " + ex.Message);
		}
	}
}
