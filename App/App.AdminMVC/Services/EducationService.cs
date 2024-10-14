using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class EducationService(IHttpClientFactory factory) : IEducationService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddEducationAsync(AddEducationDto dto)
    {
        var apiResponse = await DataApiClient.PostAsJsonAsync("add-education", dto);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Eğitim bilgisi eklenirken beklenmedik bir hata oluştu..");
        }

        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            return Result.Error("Eğitim bilgisi eklenirken beklenmedik bir hata oluştu..");
        }

        return Result.SuccessWithMessage("Eğitim bilgisi başarıyla eklendi.");
    }

    public Task<Result> ChangeEducationVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteEducationAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllEducationsDto>>> GetAllEducationsAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("all-educations");

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Eğitimler getirilirken beklenmedik bir hata oluştu..");
        }

        var result = await apiResponse.Content.ReadFromJsonAsync< Result<List<AllEducationsDto>>>();

        if (!result.IsSuccess)
        {
            return Result.Error("Eğitimler getirilirken beklenmedik bir hata oluştu..");
        }

        return Result.Success(result.Value);
    }

    public async Task<Result> UpdateEducationAsync(UpdateEducationDto dto)
    {
        var apiResponse = await DataApiClient.PutAsJsonAsync("update-education", dto);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Eğitim bilgisi güncellenirken beklenmedik bir hata oluştu..");
        }

        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            return Result.Error("Eğitim bilgisi güncellenirken beklenmedik bir hata oluştu..");
        }

        return Result.SuccessWithMessage("Eğitim bilgisi başarıyla güncellendi.");
    }
}
