﻿using App.Core.Enums;
using App.Core.Results;
using App.DTOs.AuthDtos;
using App.Services.AuthService.Abstract;
using Ardalis.Result;
using System.Net.Http.Json;
using System.Net;

namespace App.Services.AuthService.Concrete;
public class AuthService(IHttpClientFactory factory) : IAuthService
{
    private HttpClient AuthApiClient => factory.CreateClient("authApi");
    public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("forgot-password", forgotPasswordDto);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Result.Error("Girmiş olduğunuz Email adresine sahip bir kullanıcı bulunamadı!..");
                }

                return Result.Error("Şifre sıfırlama linki gönderilirken bir hata oluştu!..");
            }

            return Result.SuccessWithMessage("Şifre sıfırlama linki Email adresinize gönderildi.");
        }

        catch (Exception)
        {
            return Result.Error("Şifre sıfırlama linki gönderilirken bir hata oluştu!..");
        }
    }

    public async Task<Result> NewVerificationAsync(NewVerificationMailDto dto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("new-verification", dto);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Result.Error("Girmiş olduğunuz Email adresine sahip bir kullanıcı bulunamadı!..");
                }

                return Result.Error("Aktivasyon linki gönderilirken bir hata oluştu!..");
            }

            return Result.SuccessWithMessage("Aktivasyon linki Email adresinize gönderildi.");
        }

        catch (Exception)
        {
            return Result.Error("Aktivasyon linki gönderilirken bir hata oluştu!..");
        }
    }

    public async Task<Result<TokensDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("login", loginDto);

            Result<TokensDto>? result;

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<Result<TokensDto>>();

                if (result is null)
                {
                    return Result<TokensDto>.Error("Giriş işlemi sırasında beklenmeyen bir hata oluştu! Tekrar deneyebilirsiniz.");
                }

                return Result<TokensDto>.Success(result.Value, "Hoşgeldiniz. Giriş işlemi başarılı!");
            }

            result = await response.Content.ReadFromJsonAsync<Result<TokensDto>>();

            if (result is null)
            {
                return Result<TokensDto>.Error("Giriş işlemi sırasında bir hata oluştu!..");
            }

            if (result.Status == ResultStatus.Forbidden)
            {
                return Result<TokensDto>.Forbidden("Henüz Email adresinizi doğrulamadınız. Lütfen Email adresinize gönderilen linke tıklayarak hesabınızı aktif edin.");
            }

            if (result.Status == ResultStatus.NotFound || result.Status == ResultStatus.Invalid)
            {
                return Result<TokensDto>.Error("Hatalı Email veya Şifre!");
            }

            return Result<TokensDto>.Error("Giriş işlemi sırasında bir hata oluştu!..");
        }

        catch (Exception)
        {
            return Result<TokensDto>.Error("Giriş işlemi sırasında bir hata oluştu!..");
        }
    }

    public async Task<Result> NewPasswordAsync(NewPasswordDto dto)
    {
        try
        {
            var response = await AuthApiClient.PutAsJsonAsync("new-password", dto);

            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.NotFound("Kullanıcı bulunamadı!");
                }

                return Result.Error("Şifreniz sıfırlanırken bir hata oluştu..Tekrar sıfırlama maili gönderebilirsiniz.");
            }

            return Result.SuccessWithMessage("Şifreniz başarıyla değiştirildi. Yeni şifrenizle giriş yapabilirsiniz.");
        }
        
        catch (Exception)
        {
            return Result.Error("Şifreniz sıfırlanırken bir hata oluştu..Tekrar sıfırlama maili gönderebilirsiniz.");
        }
    }

    public async Task<Result<TokensDto>> RefreshTokenAsync(string token)
    {
        try
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

        catch (Exception)
        {
            return Result<TokensDto>.Error();
        }
    }

    public async Task<RegistrationResult> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("register", registerDto);

            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RegistrationResult>();

                if (result is null)
                {
                    return new RegistrationResult(false, "Kayıt işlemi sırasında beklenmeyen bir hata oluştu!..", RegistrationError.None);
                }
                else
                {
                    if (result.Error == RegistrationError.UsernameTaken)
                    {
                        return new RegistrationResult(false, "Bu Kullanıcı Adı zaten alınmış!..", RegistrationError.UsernameTaken);
                    }
                    else if (result.Error == RegistrationError.EmailTaken)
                    {
                        return new RegistrationResult(false, "Bu Email zaten alınmış!..", RegistrationError.EmailTaken);
                    }
                    else if(result.Error == RegistrationError.BothTaken)
                    {
                        return new RegistrationResult(false, "Bu Email ve Kullanıcı Adı zaten alınmış!..", RegistrationError.BothTaken);
                    }
                    else
                    {
                        return new RegistrationResult(false, "Kayıt işlemi sırasında beklenmeyen bir hata oluştu!..", RegistrationError.None);
                    }
                }
            }

            return new RegistrationResult(true, "Kullanıcı kaydı başarıyla gerçekleşti. Lütfen hesabınızı aktive etmek için Email adresinizi kontrol ediniz.", RegistrationError.None);
        }
        catch (Exception)
        {
            return new RegistrationResult(false, "Kayıt işlemi sırasında beklenmeyen bir hata oluştu!..", RegistrationError.None);
        }
    }

    public async Task<Result> RenewPasswordEmailAsync(RenewPasswordDto dto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("renew-password", dto);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Error("Email adresiniz doğrulanamadı!..");
            }

            return Result.SuccessWithMessage("Şifrenizi sıfırlayabilirsiniz.");
        }

        catch (Exception)
        {
            return Result.Error("Email adresiniz doğrulanırken bir problem oluştu!..");
        }
    }

    public async Task<Result> RevokeTokenAsync(string token)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("revoke-token", token);

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.SuccessWithMessage("Hesabınızdan başarıyla çıkış yapıldı.");
            }

            return Result.Error("Hesabınızdan çıkış yapılırken bir problemle karşılaşıldı.");
        }
        catch (Exception)
        {
            return Result.Error("Hesabınızdan çıkış yapılırken bir problemle karşılaşıldı.");
        }
    }

    public async Task<Result> ValidateTokenAsync(string token)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("validate-token", token);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Result>();
            }

            return Result.Error();
        }
        catch (Exception)
        {
            return Result.Error();
        }
    }

    public async Task<Result> VerifyEmailAsync(VerifyEmailDto dto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("verify-email", dto);

            if (response.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Email başarıyla doğrulandı ve hesabınız aktif edildi. Hesabınıza giriş yapabilirsiniz.");
            }

            return Result.Error("Email doğrulama başarısız!..Tekrar doğrulama maili almak için mail adresinizi girebilirsiniz.");
        }
        catch (Exception)
        {
            return Result.Error("Email doğrulama başarısız!..Tekrar doğrulama maili almak için mail adresinizi girebilirsiniz.");
        }
    }

    public Task<Result> RefreshTokenApiAsync(string token)
    {
        throw new NotImplementedException();
    }
}
