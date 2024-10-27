using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.AuthDtos;
using App.DTOs.UserDtos;
using App.Services.AuthService.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using IdentityModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.AuthAPI.Services;
public class PorfolioUserService(AuthApiDbContext authApiDb,IAuthService authService, IConfiguration configuration) : IUserPortfolioService
{
    public Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageMvcDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageApiDto dto)
    {
        try
        {
            var user = await authApiDb.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user is null)
            {
                return Result<TokensDto>.NotFound();
            }

            user.ImageUrl = dto.ImageUrl;

            authApiDb.Users.Update(user);
            await authApiDb.SaveChangesAsync();


            user.RefreshTokens.ToList().ForEach(t => t.IsRevoked = DateTime.UtcNow);

            string jwt = GenerateJwtToken(user);

            string refreshTokenString = GenerateRefreshToken();

            var refreshToken = new RefreshTokenEntity
            {
                Token = refreshTokenString,
                UserId = user.Id,
                ExpireDate = DateTime.UtcNow.AddDays(7),
            };

            await authApiDb.RefreshTokens.AddAsync(refreshToken);
            await authApiDb.SaveChangesAsync();

            var tokensDto = new TokensDto
            {
                JwtToken = jwt,
                RefreshToken = refreshTokenString
            };


            return Result<TokensDto>.Success(tokensDto);
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result<TokensDto>.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result<TokensDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<TokensDto>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result<TokensDto>> DeleteUserImageAsync(string imgUrl)
    {
        try
        {

            var user = await authApiDb.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(x => x.ImageUrl == imgUrl);

            if (user is null)
            {
                return Result<TokensDto>.NotFound();
            }

            user.ImageUrl = null;

            authApiDb.Users.Update(user);
            await authApiDb.SaveChangesAsync();

            user.RefreshTokens.ToList().ForEach(t => t.IsRevoked = DateTime.UtcNow);

            string jwt = GenerateJwtToken(user);

            string refreshTokenString = GenerateRefreshToken();

            var refreshToken = new RefreshTokenEntity
            {
                Token = refreshTokenString,
                UserId = user.Id,
                ExpireDate = DateTime.UtcNow.AddDays(7),
            };

            await authApiDb.RefreshTokens.AddAsync(refreshToken);
            await authApiDb.SaveChangesAsync();

            var tokensDto = new TokensDto
            {
                JwtToken = jwt,
                RefreshToken = refreshTokenString
            };


            return Result<TokensDto>.Success(tokensDto);
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result<TokensDto>.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result<TokensDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<TokensDto>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result<TokensDto>> EditUsernameAsync(EditUsernameDto dto)
    {
        try
        {
            var user = await authApiDb.Users.Include(u=>u.RefreshTokens).FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user is null)
            {
                return Result<TokensDto>.NotFound();
            }

            var usernameAlreadyTaken = await authApiDb.Users.FirstOrDefaultAsync(u=>u.Username == dto.Username);

            if(usernameAlreadyTaken is not null)
            {
                return Result<TokensDto>.Unavailable();
            }

            user.Username = dto.Username;

            authApiDb.Users.Update(user);
            await authApiDb.SaveChangesAsync();


            user.RefreshTokens.ToList().ForEach(t => t.IsRevoked = DateTime.UtcNow);

            string jwt = GenerateJwtToken(user);

            string refreshTokenString = GenerateRefreshToken();

            var refreshToken = new RefreshTokenEntity
            {
                Token = refreshTokenString,
                UserId = user.Id,
                ExpireDate = DateTime.UtcNow.AddDays(7),
            };

            await authApiDb.RefreshTokens.AddAsync(refreshToken);
            await authApiDb.SaveChangesAsync();

            var tokensDto = new TokensDto
            {
                JwtToken = jwt,
                RefreshToken = refreshTokenString
            };


            return Result<TokensDto>.Success(tokensDto);
        }
        catch (DbUpdateException dbUpdateEx)
        {
            return Result<TokensDto>.Error("Veritabanı güncelleme hatası: " + dbUpdateEx.Message);
        }
        catch (SqlException sqlEx)
        {
            return Result<TokensDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<TokensDto>.Error("Bir hata oluştu: " + ex.Message);
        }
    }

    private string GenerateJwtToken(UserEntity user)
    {
        var claims = new List<Claim>
           {
                new Claim(JwtClaimTypes.Subject,user.Id.ToString()),
                new Claim(JwtClaimTypes.Email,user.Email),
                new Claim(JwtClaimTypes.Role, user.Role),
                new Claim(JwtClaimTypes.Name,user.Username),
                new Claim("user-img", user.ImageUrl ?? "default-image-url"),
            };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

        var jwt = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims = claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpireMinutes")),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

        return tokenString;
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}
