using App.Core.Helpers;
using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.AuthDtos;
using App.Services;
using Ardalis.Result;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.AuthAPI.Services;
public class AuthService : IAuthService
{
    private readonly AuthApiDbContext _authApiDb;
    private readonly IConfiguration _configuration;
    public AuthService(AuthApiDbContext authApiDb, IConfiguration configuration)
    {
        _authApiDb = authApiDb;
        _configuration = configuration;
    }
    public async Task<Result<TokensDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await _authApiDb.Users.SingleOrDefaultAsync(u=>u.Email == loginDto.Email);

        if(user == null)
        {
            return Result.NotFound();
        }

        if (!HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result<TokensDto>.Error();
        }

        user.RefreshTokens.ToList().ForEach(t => t.IsRevoked = DateTime.UtcNow);

        string jwt = GenerateJwtToken(user);

        string refreshTokenString = GenerateRefreshToken();

        var refreshToken = new RefreshTokenEntity
        {
            Token = refreshTokenString,
            UserId = user.Id,
            ExpireDate = DateTime.UtcNow.AddDays(7),
        };

        await _authApiDb.RefreshTokens.AddAsync(refreshToken);
        await _authApiDb.SaveChangesAsync();

        var tokensDto = new TokensDto
        {
            AccessToken = jwt,
            RefreshToken = refreshTokenString
        };
        
        return Result<TokensDto>.Success(tokensDto);
    }

    private string GenerateJwtToken(UserEntity user)
    {
        var claims = new List<Claim>
           {
                new Claim(JwtClaimTypes.Subject,user.Id.ToString()),
                new Claim(JwtClaimTypes.Email,user.Email),
                new Claim(JwtClaimTypes.Role, user.Role)
            };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims = claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpireMinutes")),
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
