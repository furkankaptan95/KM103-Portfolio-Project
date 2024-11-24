﻿using App.DTOs.AuthDtos;
using App.DTOs.FileApiDtos;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class UserService(IHttpClientFactory factory) : IUserAdminService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    private HttpClient AuthApiClient => factory.CreateClient("authApi");
    private HttpClient FileApiClient => factory.CreateClient("fileApi");
    public async Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        try
        {
            var apiResponse = await AuthApiClient.GetAsync($"change-user-activeness-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Kullanıcının aktifliği başarıyla değiştirildi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Aktifliğini değiştirmek istediğiniz Kullanıcı bulunamadı!..";
            }
            else
            {
                errorMessage = "Kullanıcının aktifliği değiştirilirken beklenmeyen bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
       
        catch (Exception)
        {
            return Result.Error("Kullanıcının aktifliği değiştirilirken beklenmeyen bir hata oluştu..");
        }
    }

    public async Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {
        try
        {
            var apiResponse = await AuthApiClient.GetAsync("all-users");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllUsersDto>>.Error("Kullanıcılar getirilirken beklenmedik bir hata oluştu..");
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllUsersDto>>>();
            if (result is null)
            {
                return Result.Error("Kullanıcılar getirilirken beklenmedik bir hata oluştu..");
            }
                return result;
        }

        catch (Exception)
        {
            return Result.Error("Kullanıcılar getirilirken beklenmedik bir hata oluştu..");
        }
    }

    public Task<Result<string>> GetCommentsUserName(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<int>> GetUsersCount()
    {
        throw new NotImplementedException();
    }

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

            var response = await AuthApiClient.PostAsJsonAsync("change-user-image", apiDto);

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

    public async Task<Result<TokensDto>> DeleteUserImageAsync(string imageUrl)
    {
        try
        {
            var fileResponse = await FileApiClient.GetAsync($"delete-file/{imageUrl}");

            if (!fileResponse.IsSuccessStatusCode)
            {
                return Result<TokensDto>.Error("Profil Fotoğrafı silinirken bir hata oluştu!..");
            }

            var authApiResponse = await AuthApiClient.GetAsync($"delete-user-img/{imageUrl}");

            if (!authApiResponse.IsSuccessStatusCode)
            {
                if (authApiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result<TokensDto>.Error("Kullanıcı bulunamadı!..");
                }

                return Result<TokensDto>.Error("Profil Fotoğrafı silinirken bir hata oluştu!..");
            }

            var result = await authApiResponse.Content.ReadFromJsonAsync<Result<TokensDto>>();

            if (result is null)
            {
                return Result<TokensDto>.Error("Profil Fotoğrafı silinirken bir hata oluştu!..");
            }

            return Result<TokensDto>.Success(result.Value, "Profil Fotoğrafı başarıyla silindi.");

        }
        catch (Exception)
        {
            return Result<TokensDto>.Error("Profil Fotoğrafı silinirken bir hata oluştu!..");
        }
    }

    public async Task<Result<TokensDto>> EditUsernameAsync(EditUsernameDto dto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("edit-username", dto);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
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

            if (result is null)
            {
                return Result<TokensDto>.Error("Kullanıcı isminiz değiştirilirken beklenmedik bir hata oluştu!..");
            }

            return Result<TokensDto>.Success(result.Value, "Kullanıcı isminiz başarıyla güncellendi.");
        }
        catch (Exception)
        {
            return Result<TokensDto>.Error("Kullanıcı isminiz değiştirilirken beklenmedik bir hata oluştu!..");
        }
    }
}
