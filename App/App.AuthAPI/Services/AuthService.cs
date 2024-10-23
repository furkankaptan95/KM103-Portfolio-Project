using App.Core.Enums;
using App.Core.Helpers;
using App.Core.Results;
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
    private readonly IEmailService _emailService;
    public AuthService(AuthApiDbContext authApiDb, IConfiguration configuration, IEmailService emailService)
    {
        _authApiDb = authApiDb;
        _configuration = configuration;
        _emailService = emailService;
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

    public async Task<Result<TokensDto>> RefreshTokenAsync(string? token)
    {
        if(token is null)
        {
            return Result<TokensDto>.Invalid();
        }

        var refreshToken = await _authApiDb.RefreshTokens.Where(rt =>
        rt.Token == token &&
        rt.ExpireDate > DateTime.UtcNow &&
        rt.IsRevoked == null &&
        rt.IsUsed == null).Include(rt=>rt.User).FirstOrDefaultAsync();

        if (refreshToken is null)
        {
            return Result<TokensDto>.Error();
        }

        refreshToken.IsUsed = DateTime.UtcNow;

        var newJwt = GenerateJwtToken(refreshToken.User);
        var newRefreshTokenString = GenerateRefreshToken();

        var newRefreshToken = new RefreshTokenEntity
        {
            Token = newRefreshTokenString,
            UserId = refreshToken.User.Id,
            ExpireDate = DateTime.UtcNow.AddDays(7),
        };

        await _authApiDb.RefreshTokens.AddAsync(newRefreshToken);
        await _authApiDb.SaveChangesAsync();

        var response = new TokensDto
        {
            AccessToken = newJwt,
            RefreshToken = newRefreshTokenString
        };

        return Result<TokensDto>.Success(response); 
    }

    public async Task<RegistrationResult> RegisterAsync(RegisterDto registerDto)
    {
        var isEmailAlreadyTaken = await _authApiDb.Users.SingleOrDefaultAsync(u => u.Email == registerDto.Email);
        var isUsernameAlreadyTaken = await _authApiDb.Users.SingleOrDefaultAsync(u => u.Username == registerDto.Username);

        if(isEmailAlreadyTaken is not null&& isUsernameAlreadyTaken is not null)
        {
            return new RegistrationResult { IsSuccess = false, Error = RegistrationError.BothTaken };
        }
        else if (isEmailAlreadyTaken is null && isUsernameAlreadyTaken is not null)
        {
            return new RegistrationResult { IsSuccess = true, Error = RegistrationError.UsernameTaken };
        }
        else if (isEmailAlreadyTaken is not null && isUsernameAlreadyTaken is null)
        {
            return new RegistrationResult { IsSuccess = true, Error = RegistrationError.EmailTaken };
        }

        byte[] passwordHash, passwordSalt;

        HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

        var user = new UserEntity
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _authApiDb.Users.AddAsync(user);
        await _authApiDb.SaveChangesAsync();

        var token = Guid.NewGuid().ToString().Substring(0, 6);

        var userVerification = new UserVerificationEntity
        {
            UserId = user.Id,
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(24),
            CreatedAt = DateTime.UtcNow
        };

        await _authApiDb.UserVerifications.AddAsync(userVerification);
        await _authApiDb.SaveChangesAsync();

        var verificationLink = $"https://localhost:7114/verify-email?email={user.Email}&token={token}";

        var htmlMailBody = $"<h1>Lütfen Email adresinizi doğrulayın!</h1><a href='{verificationLink}'>Email Doğrulama için tıklayınız.</a>";
        var emailResult = await _emailService.SendEmailAsync(user.Email, "Kayıt başarılı. Lütfen email adresinizi doğrulayın.", htmlMailBody);

        return new RegistrationResult { IsSuccess = true };
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
