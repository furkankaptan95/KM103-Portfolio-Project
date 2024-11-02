using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace App.DataAPI.Services.PortfolioServices;
public class CommentPortfolioService : ICommentPortfolioService
{
	private readonly DataApiDbContext _dataApiDb;
	private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentPortfolioService(DataApiDbContext dataApiDb, IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
	{
		_dataApiDb = dataApiDb;
		_factory = factory;
        _httpContextAccessor = httpContextAccessor;
	}
	private HttpClient AuthApiClient => _factory.CreateClient("authApi");

    public async Task<Result> AddCommentSignedAsync(AddCommentSignedDto dto)
    {
        try
        {
            var entity = new CommentEntity()
            {
                Content = dto.Content,
                BlogPostId = dto.BlogPostId,
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

    public async Task<Result> AddCommentUnsignedAsync(AddCommentUnsignedDto dto)
    {
        try
        {
            var entity = new CommentEntity()
            {
              Content = dto.Content,
			  BlogPostId = dto.BlogPostId,
			  UnsignedCommenterName = dto.UnsignedCommenterName,
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

    public async Task<Result> DeleteCommentAsync(int id)
    {
        try
        {
            var entity = await _dataApiDb.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            int? userId = GetUserIdFromCookie();

            if(userId is null)
            {
                return Result.Unauthorized();
            }

            if(userId != entity.UserId)
            {
                return Result.Forbidden();
            }

            _dataApiDb.Comments.Remove(entity);
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
					CommenterId = item.UserId, 
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

    public int? GetUserIdFromCookie()
    {
        // Cookie'yi al
        var cookie = _httpContextAccessor.HttpContext.Request.Cookies["JwtToken"];

        if (string.IsNullOrEmpty(cookie))
        {
            return null; // Cookie yoksa null dön
        }

        // JWT token'ını çözümle
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(cookie); // Token'ı okuma

        // "sub" claim'ini al
        var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");

        // Eğer subClaim null değilse, değerini int'e çevirip döndür
        if (subClaim != null && int.TryParse(subClaim.Value, out var userId))
        {
            return userId; // Değer atanır
        }

        return null; // sub claim yoksa null döner
    }
}
