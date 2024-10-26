using App.Data.DbContexts;
using App.DTOs.UserDtos;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.AuthAPI.Services;
public class PorfolioUserService(AuthApiDbContext authApiDb) : IUserPortfolioService
{
    public async Task<Result> EditUsernameAsync(EditUsernameDto dto)
    {
        try
        {
            var user = await authApiDb.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user is null)
            {
                return Result.NotFound();
            }

            var usernameAlreadyTaken = await authApiDb.Users.FirstOrDefaultAsync(u=>u.Username == dto.Username);

            if(usernameAlreadyTaken is not null)
            {
                return Result.Unavailable();
            }

            user.Username = dto.Username;

            authApiDb.Users.Update(user);
            await authApiDb.SaveChangesAsync();

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
}
