using App.Data.DbContexts;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.AuthAPI.Services;
public class AdminUserService(AuthApiDbContext authApiDb) : IUserService
{
    public Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> GetCommentsUserName(int id)
    {
        try
        {
            var user = await authApiDb.Users.FirstOrDefaultAsync(x => x.Id == id);  

            if (user == null)
            {
                return Result.NotFound();
            }

            return Result<string>.Success(user.Username);
        }

        catch (SqlException sqlEx)
        {
            return Result.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<string>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public Task<Result<int>> GetUsersCount(int id)
    {
        throw new NotImplementedException();
    }
}
