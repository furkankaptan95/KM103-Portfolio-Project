using App.DTOs.AboutMeDtos.Admin;
using App.DTOs.FileApiDtos;
using App.DTOs.HomeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class HomeService(IHttpClientFactory factory) : IHomeAdminService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    private HttpClient FileApiClient => factory.CreateClient("fileApi");
    public async Task<Result<HomeDto>> GetHomeInfosAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("get-home-infos");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<HomeDto>>();

                if (result is null)
                {
                    return Result<HomeDto>.Error();
                }

                return result;
            }

            return Result<HomeDto>.Error();
        }
       
          catch (Exception)
        {
            return Result<HomeDto>.Error();
        }
    }

    public async Task<Result> UploadCvAsync(IFormFile cv)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            var cvContent = new StreamContent(cv.OpenReadStream());
            cvContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(cv.ContentType);
            content.Add(cvContent, "file", cv.FileName);

            var fileApiResponse = await FileApiClient.PostAsync("upload-file-general", content);

            if (!fileApiResponse.IsSuccessStatusCode)
            {
                return Result.Error("CV yüklenirken beklenmeyen bir hata oluştu.Tekrar deneyebilirsiniz.");
            }

            var url = await fileApiResponse.Content.ReadAsStringAsync();

            if (url is null)
            {
                return Result.Error("CV eklenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            var apiResponse = await DataApiClient.PostAsJsonAsync("add-cv", url);

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("CV başarıyla güncellendi.");
            }

            return Result.Error("CV yüklenirken beklenmeyen bir hata oluştu.Tekrar deneyebilirsiniz..");
        }
        catch (Exception)
        {
            return Result.Error("CV yüklenirken beklenmeyen bir hata oluştu.Tekrar deneyebilirsiniz..");
        }
    }

    public Task<Result> UploadCvAsync(string url)
    {
        throw new NotImplementedException();
    }
}
