using App.Core.Enums;
using App.Core.Helpers;
using App.Core.Results;
using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.AuthDtos;
using App.Services;
using App.Services.AuthService.Abstract;
using Ardalis.Result;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace App.AuthAPI.Services;
public class AuthService : IAuthService
{
    private readonly AuthApiDbContext _authApiDb;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthService(AuthApiDbContext authApiDb, IConfiguration configuration, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
    {
        _authApiDb = authApiDb;
        _configuration = configuration;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            UserEntity? emailToRenewPassword;

            if (forgotPasswordDto.IsAdmin == true)
            {
                emailToRenewPassword = await _authApiDb.Users.SingleOrDefaultAsync(u => u.Email == forgotPasswordDto.Email && u.Role == "admin");
            }
            else
            {
                emailToRenewPassword = await _authApiDb.Users.SingleOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);
            }

            if (emailToRenewPassword is null)
            {
                return Result.NotFound();
            }

            var token = Guid.NewGuid().ToString().Substring(0, 6);

            var forgotPassword = new UserVerificationEntity
            {
                UserId = emailToRenewPassword.Id,
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(24),
                CreatedAt = DateTime.UtcNow
            };

            await _authApiDb.UserVerifications.AddAsync(forgotPassword);
            await _authApiDb.SaveChangesAsync();

            var verificationLink = $"{forgotPasswordDto.Url}/renew-password?email={forgotPasswordDto.Email}&token={token}";

            var htmlMailBody = $"<h1>Lütfen Email adresinizi doğrulayın!</h1><a href='{verificationLink}'>Şifrenizi sıfırlamak için tıklayınız.</a>";
            var emailResult = await _emailService.SendEmailAsync(emailToRenewPassword.Email, "Lütfen email adresinizi doğrulayın.", htmlMailBody);

            if (emailResult.IsSuccess)
            {
                return Result.Success();
            }

            return Result.Error("Email gönderilirken bir hata oluştu.");
        }

        catch (Exception ex)
        {
            return Result.Error($"Bir hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<TokensDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            UserEntity? user;

            if (loginDto.IsAdmin == true)
            {
                user = await _authApiDb.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Role == "admin");
            }
            else
            {
                user = await _authApiDb.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            }
            
            if (user == null)
            {
                return Result.NotFound();
            }

            if (!HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result<TokensDto>.Invalid();
            }

            if (HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt) && user.IsActive == false)
            {
                return Result<TokensDto>.Forbidden();
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
        catch (Exception ex)
        {
            return Result<TokensDto>.Error($"Bir hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<TokensDto>> RefreshTokenAsync(string token)
    {
        try
        {
            var refreshToken = await _authApiDb.RefreshTokens.Where(rt =>
               rt.Token == token &&
               rt.ExpireDate > DateTime.UtcNow &&
               rt.IsRevoked == null &&
               rt.IsUsed == null).Include(rt => rt.User).FirstOrDefaultAsync();

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

        catch (Exception ex)
        {
            return Result<TokensDto>.Error($"Bir hata oluştu: {ex.Message}");
        }
    }

    public async Task<RegistrationResult> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var isEmailAlreadyTaken = await _authApiDb.Users.SingleOrDefaultAsync(u => u.Email == registerDto.Email);
            var isUsernameAlreadyTaken = await _authApiDb.Users.SingleOrDefaultAsync(u => u.Username == registerDto.Username);

            if (isEmailAlreadyTaken is not null && isUsernameAlreadyTaken is not null)
            {
                return new RegistrationResult(false, null, RegistrationError.BothTaken);
            }

            else if (isEmailAlreadyTaken is null && isUsernameAlreadyTaken is not null)
            {
                return new RegistrationResult(false, null, RegistrationError.UsernameTaken);
            }

            else if (isEmailAlreadyTaken is not null && isUsernameAlreadyTaken is null)
            {
                return new RegistrationResult(false, null, RegistrationError.EmailTaken);
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

            var verificationLink = $"https://localhost:7167/verify-email?email={user.Email}&token={token}";

            var htmlMailBody = $"<h1>Lütfen Email adresinizi doğrulayın!</h1><a href='{verificationLink}'>Email Doğrulama için tıklayınız.</a>";
            var emailResult = await _emailService.SendEmailAsync(user.Email, "Kayıt başarılı. Lütfen email adresinizi doğrulayın.", htmlMailBody);

            return new RegistrationResult(true, null, RegistrationError.None);
        }
        catch (Exception ex)
        {
            return new RegistrationResult(false,$"Bir hata oluştu: {ex.Message}",RegistrationError.None);
        }
    }

    public async Task<Result> RenewPasswordEmailAsync(RenewPasswordDto dto)
    {
        try
        {
            UserVerificationEntity? userVerification;

            if(dto.IsAdmin  == true)
            {
                userVerification = await _authApiDb.UserVerifications.Include(uv => uv.User).FirstOrDefaultAsync(uv => uv.User.Email == dto.Email && uv.Token == dto.Token && uv.User.Role == "admin");
            }
            else
            {
                userVerification = await _authApiDb.UserVerifications.Include(uv => uv.User).FirstOrDefaultAsync(uv => uv.User.Email == dto.Email && uv.Token == dto.Token);
            }
            
            if (userVerification == null || userVerification.Expiration < DateTime.UtcNow)
            {
                return Result.Error();
            }

            _authApiDb.UserVerifications.Remove(userVerification);
            await _authApiDb.SaveChangesAsync();

            return Result.Success();
        }

        catch (Exception ex)
        {
            return Result.Error($"Bir hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> RevokeTokenAsync(string token)
    {
        try
        {
            var refreshToken = await _authApiDb.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken is null)
            {
                return Result.NotFound();
            }

            refreshToken.IsRevoked = DateTime.UtcNow;

            _authApiDb.RefreshTokens.Update(refreshToken);
            await _authApiDb.SaveChangesAsync();

            return Result.Success();
        }

        catch (Exception ex)
        {
            return Result.Error($"Bir hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return Result.Error("Token is null or empty");
        }

        try
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                return Result.Error("JWT formatı hatalı");
            }

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
            {
                return Result.Error("Token okunamıyor veya geçersiz formatta.");
            }

            var header = parts[0];
            var payload = parts[1];
            var signature = parts[2];

            var computedSignature = CreateSignature(header, payload, _configuration["Jwt:Key"]);

            if (computedSignature == signature)
            {
                return Result.Success();
            }

            return Result.Error();
        }
        catch (Exception)
        {
            return Result.Error();
        }
    }

    private static string CreateSignature(string header, string payload, string secret)
    {
        var key = Encoding.UTF8.GetBytes(secret);

        using (var algorithm = new HMACSHA256(key))
        {
            var data = Encoding.UTF8.GetBytes(header + "." + payload);
            var hash = algorithm.ComputeHash(data);
            return Base64UrlEncode(hash);
        }
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    public async Task<Result> VerifyEmailAsync(VerifyEmailDto dto)
    {
        try
        {
            var userVerification = await _authApiDb.UserVerifications.Where(uv => uv.User.Email == dto.Email && uv.Token == dto.Token).Include(uv => uv.User).FirstOrDefaultAsync();

            if (userVerification == null || userVerification.Expiration < DateTime.UtcNow)
            {
                return Result.Invalid();
            }

            userVerification.User.IsActive = true;

            _authApiDb.UserVerifications.Update(userVerification);
            await _authApiDb.SaveChangesAsync();

            _authApiDb.UserVerifications.Remove(userVerification);
            await _authApiDb.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error($"Bir hata oluştu: {ex.Message}");
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
                new Claim("user-img", user.ImageUrl ?? "default.png"),
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

    public async Task<Result> NewPasswordAsync(NewPasswordDto dto)
    {
        try
        {
            var user = await _authApiDb.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);

            if (user is null)
            {
                return Result.NotFound();
            }

            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash(dto.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _authApiDb.Users.Update(user);
            await _authApiDb.SaveChangesAsync();

            return Result.Success();
        }

        catch (Exception ex)
        {
            return Result.Error($"Bir hata oluştu: {ex.Message}");
        }
    }
}
