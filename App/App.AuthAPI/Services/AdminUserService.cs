using App.Data.DbContexts;
using App.Data.Entities;
using App.DTOs.AuthDtos;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using IdentityModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.AuthAPI.Services;
public class AdminUserService : IUserAdminService
{
    private readonly IHttpClientFactory _factory;
    private readonly AuthApiDbContext _authApiDb;
    private readonly IConfiguration _configuration;
    public AdminUserService(AuthApiDbContext authApiDb, IHttpClientFactory factory, IConfiguration configuration)
    {
        _authApiDb = authApiDb;
        _factory = factory;
        _configuration = configuration;
    }
    private HttpClient DataApiClient => _factory.CreateClient("dataApi");
    public Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageMvcDto dto)
    {
        throw new NotImplementedException();
    }
    public async Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageApiDto dto)
    {
        try
        {
            var user = await _authApiDb.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user is null)
            {
                return Result<TokensDto>.NotFound();
            }

            user.ImageUrl = dto.ImageUrl;

            _authApiDb.Users.Update(user);
            await _authApiDb.SaveChangesAsync();

            var tokensDto = await GetTokens(user);

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
            var user = await _authApiDb.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(x => x.ImageUrl == imgUrl);

            if (user is null)
            {
                return Result<TokensDto>.NotFound();
            }

            user.ImageUrl = null;

            _authApiDb.Users.Update(user);
            await _authApiDb.SaveChangesAsync();

            var tokensDto = await GetTokens(user);

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
            var user = await _authApiDb.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user is null)
            {
                return Result<TokensDto>.NotFound();
            }

            var usernameAlreadyTaken = await _authApiDb.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (usernameAlreadyTaken is not null)
            {
                return Result<TokensDto>.Unavailable();
            }

            user.Username = dto.Username;

            _authApiDb.Users.Update(user);
            await _authApiDb.SaveChangesAsync();

            var tokensDto = await GetTokens(user);

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
    private async Task<TokensDto> GetTokens(UserEntity user)
    {
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

        return tokensDto;
    }
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
                return Result<string>.NotFound();
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
