using App.DTOs.AboutMeDtos;
using App.DTOs.ExperienceDtos;
using App.DTOs.FileApiDtos;
using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class ProjectService(IHttpClientFactory factory) : IProjectService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    private HttpClient FileApiClient => factory.CreateClient("fileApi");

    public Task<Result> AddProjectAsync(AddProjectApiDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> AddProjectAsync(AddProjectMVCDto dto)
    {
        using var content = new MultipartFormDataContent();

        var imageContent = new StreamContent(dto.ImageFile.OpenReadStream());
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile.ContentType);
        content.Add(imageContent, "imageFile1", dto.ImageFile.FileName); // "imageFile1" API'deki parametre adı ile uyumlu olmalı

        var fileApiResponse = await FileApiClient.PostAsync("upload-files", content);

        if (!fileApiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Resimler yüklenirken beklenmeyen bir hata oluştu.");
        }

        var urlDto = await fileApiResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

        var apiDto = new AddProjectApiDto
        {
            ImageUrl = urlDto.ImageUrl1,
            Title = dto.Title,
            Description = dto.Description,
        };

        var apiResponse = await DataApiClient.PostAsJsonAsync("add-project", apiDto);

        if (apiResponse.IsSuccessStatusCode)
        {
            return Result.SuccessWithMessage("Proje  başarıyla eklendi. ");
        }

        return Result.Error("Proje eklenirken beklenmeyen bir hata oluştu..");
    }

    public Task<Result> ChangeProjectVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteProjectAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllProjectsDto>>> GetAllProjectsAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("all-projects");

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result<List<AllProjectsDto>>.Error("Projeler getirilirken beklenmedik bir hata oluştu..");
        }

        return await apiResponse.Content.ReadFromJsonAsync<Result<List<AllProjectsDto>>>();
    }

    public Task<Result<ProjectToUpdateDto>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateProjectAsync(UpdateProjectMVCDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateProjectAsync(UpdateProjectApiDto dto)
    {
        throw new NotImplementedException();
    }
}
