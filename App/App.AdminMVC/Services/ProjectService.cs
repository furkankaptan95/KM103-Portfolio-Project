using App.DTOs.FileApiDtos;
using App.DTOs.ProjectDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

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
        try
        {
            using var content = new MultipartFormDataContent();

            var imageContent = new StreamContent(dto.ImageFile.OpenReadStream());
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile.ContentType);
            content.Add(imageContent, "imageFile1", dto.ImageFile.FileName); // "imageFile1" API'deki parametre adı ile uyumlu olmalı

            var fileApiResponse = await FileApiClient.PostAsync("upload-files", content);

            if (!fileApiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Resim yüklenirken beklenmeyen bir hata oluştu.");
            }

            var urlDto = await fileApiResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

            if (urlDto is null)
            {
                return Result.Error("Proje eklenirken beklenmeyen bir hata oluştu..");
            }

            var apiDto = new AddProjectApiDto
            {
                ImageUrl = urlDto.ImageUrl1,
                Title = dto.Title,
                Description = dto.Description,
            };

            var apiResponse = await DataApiClient.PostAsJsonAsync("add-project", apiDto);

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Proje başarıyla eklendi. ");
            }

            return Result.Error("Proje eklenirken beklenmeyen bir hata oluştu..");
        }

        catch (Exception)
        {
            return Result.Error("Proje eklenirken beklenmeyen bir hata oluştu..");
        }
    }

    public async Task<Result> ChangeProjectVisibilityAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"change-project-visibility-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Projenin görünürlüğü başarıyla değiştirildi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Görünürlüğünü değiştirmek istediğiniz Proje bulunamadı!..";
            }
            else
            {
                errorMessage = "Projenin görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
       
        catch (Exception)
        {
            return Result.Error("Projenin görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..");
        }
    }

    public async Task<Result> DeleteProjectAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.DeleteAsync($"delete-project-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Proje başarıyla silindi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Silmek istediğiniz Proje bilgisi bulunamadı!..";
            }
            else
            {
                errorMessage = "Proje silinirken beklenmedik bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
       
        catch (Exception)
        {
            return Result.Error("Proje silinirken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result<List<AllProjectsDto>>> GetAllProjectsAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("all-projects");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllProjectsDto>>.Error("Projeler getirilirken beklenmedik bir hata oluştu..");
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllProjectsDto>>>();

            if(result is null)
            {
                return Result<List<AllProjectsDto>>.Error("Projeler getirilirken beklenmedik bir hata oluştu..");
            }

            return result;
        }
       
        catch (Exception)
        {
            return Result<List<AllProjectsDto>>.Error("Projeler getirilirken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result<ProjectToUpdateDto>> GetByIdAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"get-project-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<ProjectToUpdateDto>>();
                if(result is null)
                {
                    return Result<ProjectToUpdateDto>.Error("Güncellemek istediğiniz Proje bilgileri getirilirken beklenmeyen bir hata oluştu.");
                }
                return result;
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Güncellemek istediğiniz Proje bilgisine ulaşılamadı!..";
            }
            else
            {
                errorMessage = "Güncellemek istediğiniz Proje bilgileri getirilirken beklenmeyen bir hata oluştu.";
            }

            return Result<ProjectToUpdateDto>.Error(errorMessage);
        }
      
        catch (Exception)
        {
            return Result<ProjectToUpdateDto>.Error("Güncellemek istediğiniz Proje bilgileri getirilirken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<Result> UpdateProjectAsync(UpdateProjectMVCDto dto)
    {
        try
        {
            var apiDto = new UpdateProjectApiDto();

            apiDto.Id = dto.Id;
            apiDto.Description = dto.Description;
            apiDto.Title = dto.Title;

            if (dto.ImageFile is not null)
            {
                using var content = new MultipartFormDataContent();

                var imageContent = new StreamContent(dto.ImageFile.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile.ContentType);
                content.Add(imageContent, "imageFile1", dto.ImageFile.FileName); // "imageFile1" API'deki parametre adı ile uyumlu olmalı

                var fileApiResponse = await FileApiClient.PostAsync("upload-files", content);

                if (!fileApiResponse.IsSuccessStatusCode)
                {
                    return Result.Error("Resim yüklenirken beklenmeyen bir hata oluştu.");
                }

                var urlDto = await fileApiResponse.Content.ReadFromJsonAsync<ReturnUrlDto>();

                if(urlDto is null)
                {
                    return Result.Error("Proje güncellenirken beklenmeyen bir hata oluştu.. Tekrar güncellemeyi deneyebilirsiniz.");
                }

                apiDto.ImageUrl = urlDto.ImageUrl1;
            }

            var dataApiResponse = await DataApiClient.PutAsJsonAsync("update-project", apiDto);

            if (dataApiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage(" Proje bilgileri başarılı bir şekilde güncellendi. ");
            }

            if (dataApiResponse.StatusCode ==HttpStatusCode.NotFound)
            {
                return Result.NotFound("Güncellemek istediğiniz Proje bulunamadı!");
            }

            return Result.Error("Proje güncellenirken beklenmeyen bir hata oluştu.. Tekrar güncellemeyi deneyebilirsiniz.");
        }
       
        catch (Exception)
        {
            return Result.Error("Proje güncellenirken beklenmeyen bir hata oluştu.. Tekrar güncellemeyi deneyebilirsiniz.");
        }
    }  

    public Task<Result> UpdateProjectAsync(UpdateProjectApiDto dto)
    {
        throw new NotImplementedException();
    }
}
