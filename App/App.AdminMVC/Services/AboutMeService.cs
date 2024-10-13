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
        
        var fileResponse = await FileApiClient.PostAsync("upload-files", content);

        // var errorList = new ErrorList(new List<string>()); Çoklu mesaj dönülmek istenirse bu şekilde eklenebilir.


        if (!fileResponse.IsSuccessStatusCode)
        {
            return Result.Error("Resimler yüklenirken beklenmeyen bir hata oluştu.");
        }

        var urlDto = await fileResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

        var apiDto = new AddAboutMeApiDto
        {
            ImageUrl1 = urlDto.ImageUrl1,
            ImageUrl2 = urlDto.ImageUrl2,
            Introduction = dto.Introduction,
        };

        var apiResponse = await DataApiClient.PostAsJsonAsync("add-about-me", apiDto);

        return await apiResponse.Content.ReadFromJsonAsync<Result>();
    }

    public Task<Result> AddAboutMeAsync(AddAboutMeApiDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ShowAboutMeDto>> GetAboutMeAsync()
    {
        var response = await DataApiClient.GetAsync("get-about-me");

        return await response.Content.ReadFromJsonAsync<Result<ShowAboutMeDto>>();
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
            var fileResponse = await FileApiClient.PostAsync("upload-files", content);

            if (!fileResponse.IsSuccessStatusCode)
            {
                return Result.Error("Resimler yüklenirken beklenmeyen bir hata oluştu.");
            }

            var urlDto = await fileResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

            updateApiDto.ImageUrl1 = urlDto.ImageUrl1;
            updateApiDto.ImageUrl2 = urlDto.ImageUrl2;
           
        }

        var apiResponse = await DataApiClient.PostAsJsonAsync("update-about-me", updateApiDto);

        return await apiResponse.Content.ReadFromJsonAsync<Result>();
    }
}
