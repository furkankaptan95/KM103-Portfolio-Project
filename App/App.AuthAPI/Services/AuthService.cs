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

    public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var emailToRenewPassword = await _authApiDb.Users.SingleOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);

        if (emailToRenewPassword is null)
        {
            return Result.NotFound();
        }

        var verificationCode = Guid.NewGuid().ToString().Substring(0, 6);

        var forgotPassword = new UserVerificationEntity
        {
            UserId = emailToRenewPassword.Id,
            Token = verificationCode,
            Expiration = DateTime.UtcNow.AddHours(24),
            CreatedAt = DateTime.UtcNow
        };

        await _authApiDb.UserVerifications.AddAsync(forgotPassword);
        await _authApiDb.SaveChangesAsync();


        var verificationLink = $"https://localhost:7114/renew-password/{verificationCode}";

        var htmlMailBody = $"<h1>Lütfen Email adresinizi doğrulayın!</h1><a href='{verificationLink}'>Şifrenizi sıfırlamak için tıklayınız.</a>";
        var emailResult = await _emailService.SendEmailAsync(emailToRenewPassword.Email, "Lütfen email adresinizi doğrulayın.", htmlMailBody);

        return Result.Success();
    }

    public async Task<Result<TokensDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await _authApiDb.Users.Include(u=>u.RefreshTokens).FirstOrDefaultAsync(u=>u.Email == loginDto.Email);

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
            JwtToken = jwt,
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
            JwtToken = newJwt,
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

    public async Task<Result> RenewPasswordEmailAsync(string email, string token)
    {
        var userVerification = await _authApiDb.UserVerifications.Include(uv=>uv.User).FirstOrDefaultAsync(uv => uv.User.Email == email && uv.Token == token);

        if (userVerification == null || userVerification.Expiration < DateTime.UtcNow)
        {
            return Result.Error();
        }


        _authApiDb.UserVerifications.Remove(userVerification);
        await _authApiDb.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> RevokeTokenAsync(string token)
    {
        var refreshToken = await _authApiDb.RefreshTokens.FirstOrDefaultAsync(rt=>rt.Token == token);

        if (refreshToken is null)
        {
            return Result.NotFound();
        }

        refreshToken.IsRevoked = DateTime.UtcNow;

        _authApiDb.RefreshTokens.Update(refreshToken);
        await _authApiDb.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return Result.Error("Token is null or empty");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretKey,
                ValidateIssuer = false, // Gerekirse kontrol edin
                ValidateAudience = false, // Gerekirse kontrol edin
                ClockSkew = TimeSpan.Zero // Geçerlilik süresi kontrolü için tolerans süresi
            }, out SecurityToken validatedToken);

            // Token geçerli ise, Success döner
            return Result.Success();
        }
        catch (SecurityTokenExpiredException)
        {
            // Token süresi dolmuş
            return Result.Error("Token has expired");
        }
        catch (SecurityTokenException)
        {
            // Token geçersiz
            return Result.Error("Invalid token");
        }
        catch (Exception ex)
        {
            // Diğer hatalar
            return Result.Error($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Result> VerifyEmailAsync(string email, string token)
    {
        var userVerification = await _authApiDb.UserVerifications.Where(uv => uv.User.Email == email && uv.Token == token).Include(uv=>uv.User).FirstOrDefaultAsync();

        if (userVerification == null || userVerification.Expiration < DateTime.UtcNow)
        {
            return Result.Error();
        }

        userVerification.User.IsActive = true;
        _authApiDb.UserVerifications.Update(userVerification);
        await _authApiDb.SaveChangesAsync();
        _authApiDb.UserVerifications.Remove(userVerification);
        await _authApiDb.SaveChangesAsync();

        return Result.Success();
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
