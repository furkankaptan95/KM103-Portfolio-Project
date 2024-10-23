using App.Core.Enums;
using App.Core.Results;
using App.DTOs.AuthDtos;
using App.Services;
using Ardalis.Result;
using Newtonsoft.Json.Linq;
using System.Net;

namespace App.AdminMVC.Services;
public class AuthService(IHttpClientFactory factory) : IAuthService
{
    private HttpClient AuthApiClient => factory.CreateClient("authApi");
    public Task<Result> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<TokensDto>> LoginAsync(LoginDto loginDto)
    {
        var response = await AuthApiClient.PostAsJsonAsync("login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<TokensDto>>();

            if(result is null)
            {
                return Result<TokensDto>.Error();
            }
            return result;
        }
        if(response.StatusCode == HttpStatusCode.NotFound)
        {
            return Result<TokensDto>.NotFound();
        }

        return Result<TokensDto>.Error();
    }
    
    public async Task<Result<TokensDto>> RefreshTokenAsync(string token)
    {
        var response = await AuthApiClient.PostAsJsonAsync("refresh-token", token);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<TokensDto>>();

            if (result is null)
            {
                return Result<TokensDto>.Error();
            }

            return result;
        }

        return Result<TokensDto>.Error();
    }

    public async Task<RegistrationResult> RegisterAsync(RegisterDto registerDto)
    {
        var response = await AuthApiClient.PostAsJsonAsync("register", registerDto);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<RegistrationResult>();

            if(result is null)
            {
                return new RegistrationResult(false,null,RegistrationError.None);
            }
            else
            {
                if(result.Error == RegistrationError.UsernameTaken)
                {
                    return new RegistrationResult(false,"Bu Kullanıcı Adı zaten alınmış!..",RegistrationError.UsernameTaken);
                }
                else if(result.Error == RegistrationError.EmailTaken)
                {
                    return new RegistrationResult(false, "Bu Email zaten alınmış!..",RegistrationError.EmailTaken);
                }
                else
                {
                    return new RegistrationResult(false,"Bu Email ve Kullanıcı Adı zaten alınmış!..", RegistrationError.BothTaken);
                }
            }
        }

        return new RegistrationResult(true,"Kullanıcı kaydı başarıyla gerçekleşti. Lütfen hesabınızı aktive etmek için Email adresinizi kontrol ediniz.",RegistrationError.None);

    }

    public Task<Result> RenewPasswordEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RevokeTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ValidateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<Result> VerifyEmailAsync(VerifyEmailDto dto)
    {
        throw new NotImplementedException();
    }
}
