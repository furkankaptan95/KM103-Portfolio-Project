using App.DTOs.AuthDtos;
using App.DTOs.FileApiDtos;
using App.DTOs.UserDtos;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.PortfolioMVC.Services;
public class UserPortfolioService(IHttpClientFactory factory) : IUserPortfolioService
{
    private HttpClient AuthApiClient => factory.CreateClient("authApi");
    private HttpClient FileApiClient => factory.CreateClient("fileApi");

    public async Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageMvcDto dto)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            var imageContent = new StreamContent(dto.ImageFile.OpenReadStream());
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile.ContentType);
            content.Add(imageContent, "imageFile1", dto.ImageFile.FileName); // "imageFile1" API'deki parametre adı ile uyumlu olmalı

            var fileApiResponse = await FileApiClient.PostAsync("upload-files", content);

            if (!fileApiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Resim yüklenirken beklenmeyen bir hata oluştu.Tekrar deneyebilirsiniz.");
            }

            var urlDto = await fileApiResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

            if (urlDto is null)
            {
                return Result.Error("Profil Resmi güncellenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            var apiDto = new EditUserImageApiDto
            {
                Email = dto.Email,
                ImageUrl = urlDto.ImageUrl1
            };

            var response = await AuthApiClient.PostAsJsonAsync("edit-user-image", apiDto);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result<TokensDto>.Error("Kullanıcı bulunamadı!..");
                }

                return Result<TokensDto>.Error("Profil fotoğrafınız değiştirilirken beklenmedik bir hata oluştu!..");
            }

            var result = await response.Content.ReadFromJsonAsync<Result<TokensDto>>();

            if (result is null)
            {
                return Result<TokensDto>.Error("Profil fotoğrafınız değiştirilirken beklenmedik bir hata oluştu!..");
            }

            return Result<TokensDto>.Success(result.Value, "Profil fotoğrafınız başarıyla güncellendi.");
        }
        catch (Exception)
        {
            return Result<TokensDto>.Error("Profil fotoğrafınız değiştirilirken beklenmedik bir hata oluştu!..");
        }
    }

    public Task<Result<TokensDto>> ChangeUserImageAsync(EditUserImageApiDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<TokensDto>> EditUsernameAsync(EditUsernameDto dto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("edit-username", dto);

            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Result<TokensDto>.Error("Bu Kullanıcı Adı daha önce zaten alınmış!..");
                }

                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result<TokensDto>.Error("Kullanıcı bulunamadı!..");
                }

                return Result<TokensDto>.Error("Kullanıcı isminiz değiştirilirken beklenmedik bir hata oluştu!..");
            }

            var result = await response.Content.ReadFromJsonAsync<Result<TokensDto>>();

            if(result is null)
            {
                return Result<TokensDto>.Error("Kullanıcı isminiz değiştirilirken beklenmedik bir hata oluştu!..");
            }

            return Result<TokensDto>.Success(result.Value,"Kullanıcı isminiz başarıyla güncellendi.");
        }
        catch (Exception)
        {
            return Result<TokensDto>.Error("Kullanıcı isminiz değiştirilirken beklenmedik bir hata oluştu!..");
        }
    }
}
