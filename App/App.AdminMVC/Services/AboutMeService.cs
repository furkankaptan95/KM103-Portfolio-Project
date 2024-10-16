using App.DTOs.AboutMeDtos;
using App.DTOs.FileApiDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class AboutMeService : IAboutMeService
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
            return Result.Error("Resimler yüklenirken beklenmeyen bir hata oluştu.");
        }

        var urlDto = await fileApiResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

        var apiDto = new AddAboutMeApiDto
        {
            ImageUrl1 = urlDto.ImageUrl1,
            ImageUrl2 = urlDto.ImageUrl2,
            Introduction = dto.Introduction,
        };

        var apiResponse = await DataApiClient.PostAsJsonAsync("add-about-me", apiDto);

        if (apiResponse.IsSuccessStatusCode)
        {
            return Result.SuccessWithMessage(" - Hakkımda - bilgileri başarıyla eklendi. ");
        }

        return Result.Error("Hakkımda bilgileri eklenirken beklenmeyen bir hata oluştu..");
    }

    public Task<Result> AddAboutMeAsync(AddAboutMeApiDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ShowAboutMeDto>> GetAboutMeAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("get-about-me");

        if (apiResponse.IsSuccessStatusCode)
        {
            return await apiResponse.Content.ReadFromJsonAsync<Result<ShowAboutMeDto>>();
        }

       var result = await apiResponse.Content.ReadFromJsonAsync<Result<ShowAboutMeDto>>();

            string errorMessage;

            if (result.Status == ResultStatus.NotFound)
            {
                errorMessage = "Hakkımda bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurabilirsiniz.";

                return Result<ShowAboutMeDto>.NotFound(errorMessage);
            }

            errorMessage = "Bilgiler getirilirken beklenmeyen bir hata oluştu.";

            return Result<ShowAboutMeDto>.Error(errorMessage);
    }

    public Task<Result> UpdateAboutMeAsync(UpdateAboutMeApiDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateAboutMeAsync(UpdateAboutMeMVCDto dto)
    {

        var updateApiDto = new UpdateAboutMeApiDto()
        {
            Introduction = dto.Introduction,
        };

        using var content = new MultipartFormDataContent();

        if(dto.ImageFile1 is not null)
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
                return Result.Error("Resimler yüklenirken beklenmeyen bir hata oluştu.");
            }

            var urlDto = await fileApiResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

            updateApiDto.ImageUrl1 = urlDto.ImageUrl1;
            updateApiDto.ImageUrl2 = urlDto.ImageUrl2;
        }

        var dataApiResponse = await DataApiClient.PutAsJsonAsync("update-about-me", updateApiDto);

        if (!dataApiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu.. Tekrar güncellemeyi deneyebilirsiniz.");
        }

        return Result.SuccessWithMessage(" -Hakkımda- bilgileriniz başarılı bir şekilde güncellendi. ");
    }
}
