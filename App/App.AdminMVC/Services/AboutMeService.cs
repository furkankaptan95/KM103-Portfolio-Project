using App.DTOs.AboutMeDtos;
using App.DTOs.FileApiDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using System.Net;

namespace App.AdminMVC.Services;
public class AboutMeService : IAboutMeService
{
    private readonly IHttpClientFactory _factory;
    private readonly IValidator<AddAboutMeMVCDto> _addValidator;
    private readonly IValidator<ReturnUrlDto> _returnUrlValidator;
    private readonly IValidator<UpdateAboutMeMVCDto> _updateValidator;

    // Primary constructor
    public AboutMeService(IHttpClientFactory factory, IValidator<AddAboutMeMVCDto> addValidator, IValidator<ReturnUrlDto> returnUrlValidator, IValidator<UpdateAboutMeMVCDto> updateValidator)
    {
        _factory = factory;
        _addValidator = addValidator;
        _returnUrlValidator = returnUrlValidator;
        _updateValidator = updateValidator;
    }

    private HttpClient DataApiClient => _factory.CreateClient("dataApi");
    private HttpClient FileApiClient => _factory.CreateClient("fileApi");

    public async Task<Result> AddAboutMeAsync(AddAboutMeMVCDto dto)
    {
        var validationResult = await _addValidator.ValidateAsync(dto);

        // Eğer doğrulama başarısızsa, uygun bir sonuç döndür
        if (!validationResult.IsValid)
        {
            // Hataları bir Result nesnesi ile dönebilirsiniz
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Error(errorMessage);
        }

        using var content = new MultipartFormDataContent();

            var imageContent1 = new StreamContent(dto.ImageFile1.OpenReadStream());
            imageContent1.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile1.ContentType);
            content.Add(imageContent1, "imageFile1", dto.ImageFile1.FileName); // "imageFile1" API'deki parametre adı ile uyumlu olmalı
        
            var imageContent2 = new StreamContent(dto.ImageFile2.OpenReadStream());
            imageContent2.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile2.ContentType);
            content.Add(imageContent2, "imageFile2", dto.ImageFile2.FileName); // "imageFile2" API'deki parametre adı ile uyumlu olmalı
        
        var fileResponse = await FileApiClient.PostAsync("upload-files", content);

        if (!fileResponse.IsSuccessStatusCode)
        {
            return Result.Error();
        }

        var urlDto = await fileResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

        var returnUrlValidationResult = await _returnUrlValidator.ValidateAsync(urlDto);

        // Eğer doğrulama başarısızsa, uygun bir sonuç döndür
        if (!returnUrlValidationResult.IsValid)
        {
            // Hataları bir Result nesnesi ile dönebilirsiniz
            var errorMessage = string.Join(", ", returnUrlValidationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Error(errorMessage);
        }

        var apiDto = new AddAboutMeApiDto
        {
            ImageUrl1 = urlDto.ImageUrl1,
            ImageUrl2 = urlDto.ImageUrl2,
            Introduction = dto.Introduction,
        };

        var response = await DataApiClient.PostAsJsonAsync("add-about-me", apiDto);

        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public Task<Result> AddAboutMeAsync(AddAboutMeApiDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ShowAboutMeDto>> GetAboutMeAsync()
    {
        var response = await DataApiClient.GetAsync("get-about-me");

        if (response.IsSuccessStatusCode)
        {
            var dto = await response.Content.ReadFromJsonAsync<ShowAboutMeDto>();

            return Result<ShowAboutMeDto>.Success(dto);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return Result<ShowAboutMeDto>.NotFound("Hakkımda bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurunuz.");
        }

        // Diğer hata durumları için uygun bir sonuç döndür
        return Result<ShowAboutMeDto>.Error("Beklenmeyen bir hata oluştu.");
    }

    public Task<Result> UpdateAboutMeAsync(UpdateAboutMeApiDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAboutMeAsync(UpdateAboutMeMVCDto dto)
    {
        throw new NotImplementedException();
    }
}
