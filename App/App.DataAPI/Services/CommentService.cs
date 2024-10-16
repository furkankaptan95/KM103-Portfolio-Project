using App.Data.DbContexts;
using App.DTOs.CommentDtos;
using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services;
public class CommentService : ICommentService
{
    private readonly DataApiDbContext _dataApiDb;
    private readonly IHttpClientFactory _factory;

    public CommentService(DataApiDbContext dataApiDb, IHttpClientFactory factory)
    {
        _dataApiDb = dataApiDb;
        _factory = factory;
    }

    private HttpClient AuthApiClient => _factory.CreateClient("authApi");

    public Task<Result> ApproveOrNotApproveCommentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteCommentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllCommentsDto>>> GetAllCommentsAsync()
    {
        try
        {
            var dtos = new List<AllCommentsDto>();

            var entities = await _dataApiDb.Comments.Include(c=>c.BlogPost).ToListAsync();

            if (entities is null)
            {
                return Result<List<AllCommentsDto>>.Success(dtos);
            }

            foreach (var item in entities)
            {
                string commenter;

                // Eğer UnsignedCommenterName varsa onu kullan
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

                            commenter = result.Value;
                        }
                    }
                    else
                    {
                        commenter = "Anonymous";
                    }
                }

                var dto = new AllCommentsDto
                {
                    Id = item.Id,
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    IsApproved = item.IsApproved,
                    BlogPostName = item.BlogPost.Title,
                    Commenter = commenter
                };

                dtos.Add(dto);
            }

            return Result<List<AllCommentsDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllCommentsDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllCommentsDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
}
