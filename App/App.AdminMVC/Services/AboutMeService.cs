using App.DTOs.AboutMeDtos;
using App.DTOs.AboutMeDtos.Admin;
using App.DTOs.FileApiDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class AboutMeService : IAboutMeAdminService
{
    private readonly IHttpClientFactory _factory;
    public AboutMeService(IHttpClientFactory factory)
    {
        _factory = factory;
    }
    private HttpClient DataApiClient => _factory.CreateClient("dataApi");
    private HttpClient FileApiClient => _factory.CreateClient("fileApi");
    public async Task<Result> AddAboutMeAsync(AddAboutMeMVCDto dto)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            var imageContent1 = new StreamContent(dto.ImageFile1.OpenReadStream());
            imageContent1.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile1.ContentType);
            content.Add(imageContent1, "imageFile1", dto.ImageFile1.FileName); // "imageFile1" API'deki parametre adı ile uyumlu olmalı

            var imageContent2 = new StreamContent(dto.ImageFile2.OpenReadStream());
            imageContent2.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile2.ContentType);
            content.Add(imageContent2, "imageFile2", dto.ImageFile2.FileName); // "imageFile2" API'deki parametre adı ile uyumlu olmalı

            var fileApiResponse = await FileApiClient.PostAsync("upload-files", content);

            if (!fileApiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Resimler yüklenirken beklenmeyen bir hata oluştu.Tekrar deneyebilirsiniz.");
            }

            var urlDto = await fileApiResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

            if(urlDto is null)
            {
                return Result.Error("Hakkımda bilgileri eklenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            var apiDto = new AddAboutMeApiDto
            {
                ImageUrl1 = urlDto.ImageUrl1,
                ImageUrl2 = urlDto.ImageUrl2,
                Introduction = dto.Introduction,
                Field = dto.Field,
                FullName = dto.FullName,
            };

            var apiResponse = await DataApiClient.PostAsJsonAsync("add-about-me", apiDto);

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage(" - Hakkımda - bilgileri başarıyla eklendi. ");
            }

            if(apiResponse.StatusCode == HttpStatusCode.Conflict)
            {
                return Result.Conflict(" - Hakkımda - kısmına daha önceden zaten ekleme yapılmış!..");
            }

            return Result.Error("Hakkımda bilgileri eklenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
        catch (Exception)
        {
            return Result.Error("Hakkımda bilgileri eklenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }
    public Task<Result> AddAboutMeAsync(AddAboutMeApiDto dto)
    {
        throw new NotImplementedException();
    }
    public async Task<Result<bool>> CheckAboutMeAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("check-about-me");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<bool>>();

                if (result is null)
                {
                    return Result<bool>.Error();
                }

                return result;
            }

            return Result<bool>.Error();
        }
        catch (Exception)
        {
            return Result<bool>.Error();
        }
    }
    public async Task<Result<AboutMeAdminDto>> GetAboutMeAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("get-about-me");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<AboutMeAdminDto>>();

                if (result is null)
                {
                    return Result<AboutMeAdminDto>.Error("Bilgiler getirilirken beklenmeyen bir hata oluştu.");
                }

                return result;
            }
            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Hakkımda bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurabilirsiniz.";

                return Result<AboutMeAdminDto>.NotFound(errorMessage);
            }

            errorMessage = "Bilgiler getirilirken beklenmeyen bir hata oluştu.";

            return Result<AboutMeAdminDto>.Error(errorMessage);
        }
        catch (Exception)
        {
            return Result<AboutMeAdminDto>.Error("Bilgiler getirilirken beklenmeyen bir hata oluştu.");
        }
    }
    public Task<Result> UpdateAboutMeAsync(UpdateAboutMeApiDto dto)
    {
        throw new NotImplementedException();
    }
    public async Task<Result> UpdateAboutMeAsync(UpdateAboutMeMVCDto dto)
    {
        try
        {
            var updateApiDto = new UpdateAboutMeApiDto()
            {
                Introduction = dto.Introduction,
                Field = dto.Field,
                FullName = dto.FullName,
            };

            using var content = new MultipartFormDataContent();

            if (dto.ImageFile1 is not null)
            {
                var imageContent1 = new StreamContent(dto.ImageFile1.OpenReadStream());
                imageContent1.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile1.ContentType);
                content.Add(imageContent1, "imageFile1", dto.ImageFile1.FileName); // "imageFile1" API'deki parametre adı ile uyumlu olmalı
            }

            if (dto.ImageFile2 is not null)
            {
                var imageContent2 = new StreamContent(dto.ImageFile2.OpenReadStream());
                imageContent2.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile2.ContentType);
                content.Add(imageContent2, "imageFile2", dto.ImageFile2.FileName); // "imageFile2" API'deki parametre adı ile uyumlu olmalı
            }

            if (dto.ImageFile1 != null || dto.ImageFile2 != null)
            {
                var fileApiResponse = await FileApiClient.PostAsync("upload-files", content);

                if (!fileApiResponse.IsSuccessStatusCode)
                {
                    return Result.Error("Resimler yüklenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
                }

                var urlDto = await fileApiResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

                if (urlDto == null)
                {
                    return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
                }

                updateApiDto.ImageUrl1 = urlDto.ImageUrl1;
                updateApiDto.ImageUrl2 = urlDto.ImageUrl2;
            }

            var dataApiResponse = await DataApiClient.PutAsJsonAsync("update-about-me", updateApiDto);

            if (!dataApiResponse.IsSuccessStatusCode)
            {
                if(dataApiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.NotFound("Güncellemek istediğiniz -Hakkımda- kısmında bilgi bulunmuyor.");
                }
                return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage(" -Hakkımda- bilgileriniz başarılı bir şekilde güncellendi. ");
        }
        catch (Exception)
        {
            return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }
}
