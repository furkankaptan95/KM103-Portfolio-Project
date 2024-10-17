using App.Data.DbContexts;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.AuthAPI.Services;
public class AdminUserService : IUserService
{
    private readonly IHttpClientFactory _factory;
    private readonly AuthApiDbContext _authApiDb;
    public AdminUserService(AuthApiDbContext authApiDb, IHttpClientFactory factory)
    {
        _authApiDb = authApiDb;
        _factory = factory;
    }

    private HttpClient DataApiClient => _factory.CreateClient("dataApi");

    public async Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        try
        {
            var entity = await _authApiDb.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return Result.NotFound();
            }

            entity.IsActive = !entity.IsActive;

            _authApiDb.Users.Update(entity);
            await _authApiDb.SaveChangesAsync();

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

    public async Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {   
        try
        {
            var dtos = new List<AllUsersDto>();

            var entities = await _authApiDb.Users.ToListAsync();

            if (entities is null)
            {
                return Result<List<AllUsersDto>>.Success(dtos);
            }

            foreach (var item in entities)
            {
                var userComments = new List<UsersCommentsDto>();
                  
                var dataApiResponse = await DataApiClient.GetAsync($"get-users-comments-{item.Id}");

                if (dataApiResponse.IsSuccessStatusCode)
                {
                    var result = await dataApiResponse.Content.ReadFromJsonAsync<Result<List<UsersCommentsDto>>>();

                    if (result is not null)
                    {
                        userComments = result.Value;
                    }
                }

                var dto = new AllUsersDto
                {
                    Id = item.Id,
                    Username = item.Username,
                    Email = item.Email,
                    IsActive = item.IsActive,
                    ImageUrl = item.ImageUrl,
                    Comments = userComments
                };

                dtos.Add(dto);
            }

            return Result<List<AllUsersDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<AllUsersDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<AllUsersDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result<string>> GetCommentsUserName(int id)
    {
        try
        {
            var user = await _authApiDb.Users.FirstOrDefaultAsync(x => x.Id == id);  

            if (user == null)
            {
                return Result.NotFound();
            }

            return Result<string>.Success(user.Username);
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

    public async Task<Result<int>> GetUsersCount()
    {
        try
        {
            var usersCount = await _authApiDb.Users.CountAsync();

            return Result<int>.Success(usersCount);
        }
        catch (SqlException sqlEx)
        {
            return Result<int>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<int>.Error("Bir hata oluştu: " + ex.Message);
        }
    }
}
