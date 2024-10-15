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

        return Result.SuccessWithMessage("Eğitim bilgisi başarıyla eklendi.");
    }

    public async Task<Result> ChangeEducationVisibilityAsync(int id)
    {
        var apiResponse = await DataApiClient.GetAsync($"change-education-visibility-{id}");

        if (apiResponse.IsSuccessStatusCode)
        {
            return Result.SuccessWithMessage("Eğitim'in görünürlüğü başarıyla değiştirildi.");
        }

        string errorMessage;
        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (result.Status == ResultStatus.NotFound)
        {
            errorMessage = "Görünürlüğünü değiştirmek istediğiniz Eğitim bulunamadı!..";
        }
        else
        {
            errorMessage = "Eğitim'in görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
        }

        return Result.Error(errorMessage);
    }

    public async Task<Result> DeleteEducationAsync(int id)
    {
        var apiResponse = await DataApiClient.DeleteAsync($"delete-education-{id}");

        if (apiResponse.IsSuccessStatusCode)
        {
            return Result.SuccessWithMessage("Eğitim bilgisi başarıyla silindi.");
        }

        string errorMessage;
        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (result.Status == ResultStatus.NotFound)
        {
            errorMessage = "Silmek istediğiniz Eğitim bilgisi bulunamadı!..";
        }
        else
        {
            errorMessage = "Eğitim bilgisi silinirken beklenmedik bir hata oluştu..";
        }
       
        return Result.Error(errorMessage);
    }

    public async Task<Result<List<AllEducationsDto>>> GetAllEducationsAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("all-educations");

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result<List<AllEducationsDto>>.Error("Eğitimler getirilirken beklenmedik bir hata oluştu..");
        }

        return await apiResponse.Content.ReadFromJsonAsync<Result<List<AllEducationsDto>>>();
    }

    public async Task<Result<EducationToUpdateDto>> GetEducationByIdAsync(int id)
    {
        var apiResponse = await DataApiClient.GetAsync($"get-education-{id}");

        if (apiResponse.IsSuccessStatusCode)
        {
            return await apiResponse.Content.ReadFromJsonAsync<Result<EducationToUpdateDto>>();
        }

        string errorMessage;
        var result = await apiResponse.Content.ReadFromJsonAsync<Result<EducationToUpdateDto>>();

        if(result.Status == ResultStatus.NotFound)
        {
            errorMessage = "Güncellemek istediğiniz Eğitim bilgisine ulaşılamadı!..";
        }
        else
        {
            errorMessage = "Güncellemek istediğiniz Eğitim bilgileri getirilirken beklenmeyen bir hata oluştu.";
        }

        return Result<EducationToUpdateDto>.Error(errorMessage);
    }

    public async Task<Result> UpdateEducationAsync(UpdateEducationDto dto)
    {
        var apiResponse = await DataApiClient.PutAsJsonAsync("update-education", dto);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..");
        }

        return Result.SuccessWithMessage("Eğitim bilgileri başarıyla güncellendi.");
    }
}
