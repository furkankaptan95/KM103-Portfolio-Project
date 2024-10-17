using App.Data.DbContexts;
using App.DTOs.CommentDtos;
using App.DTOs.UserDtos;
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
    public async Task<Result> ApproveOrNotApproveCommentAsync(int id)
    {
        try
        {
            var entity = await _dataApiDb.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.IsApproved = !entity.IsApproved;

            _dataApiDb.Comments.Update(entity);
            await _dataApiDb.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException dbEx)
        {
            return Result.Error("Veritabanı hatası: " + dbEx.Message);
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

                            if(result is null)
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
    public async Task<Result<List<UsersCommentsDto>>> GetUsersCommentsAsync(int id)
    {
        try
        {
            var usersComments = new List<UsersCommentsDto>();

            var comments = await _dataApiDb.Comments.Where(c=>c.UserId == id).Include(c=>c.BlogPost).ToListAsync();

            if(comments.Count > 0)
            {        
                foreach (var comment in comments)
                {
                    UsersCommentsDto dto = new();

                    dto.BlogPostName = comment.BlogPost.Title;
                    dto.Content = comment.Content;
                    dto.CreatedAt = comment.CreatedAt;
                    dto.IsApproved = comment.IsApproved;

                    usersComments.Add(dto);
                }
            }

            return Result<List<UsersCommentsDto>>.Success(usersComments);
        }

        catch (SqlException sqlEx)
        {
            return Result<List<UsersCommentsDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<UsersCommentsDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
}
