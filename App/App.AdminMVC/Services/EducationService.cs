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

    public async Task<Result> ChangeEducationVisibilityAsync(int id)
    {
        var apiResponse = await DataApiClient.GetAsync($"change-education-visibility-{id}");

        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            string errorMessage;

            if (result.Status == ResultStatus.NotFound)
            {
                errorMessage = "Görünürlüğünü değiştirmek istediğiniz Eğitim bulunamadı!..";
            }

            errorMessage = "Eğitim'in görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            return Result.Error(errorMessage);
        }

        return Result.SuccessWithMessage("Eğitim'in görünürlüğü başarıyla değiştirildi.");
    }

    public async Task<Result> DeleteEducationAsync(int id)
    {
        var apiResponse = await DataApiClient.DeleteAsync($"delete-education-{id}");

        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            string errorMessage;

            if (result.Status == ResultStatus.NotFound)
            {
                errorMessage = "Silmek istediğiniz Eğitim bilgisi bulunamadı!..";
            }

            errorMessage = "Eğitim bilgisi silinirken beklenmedik bir hata oluştu..";

            return Result.Error(errorMessage);
        }

        return Result.SuccessWithMessage("Eğitim bilgisi başarıyla silindi.");
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

    public async Task<Result<EducationToUpdateDto>> GetEducationByIdAsync(int id)
    {
        var apiResponse = await DataApiClient.GetAsync($"get-education-{id}");

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result<EducationToUpdateDto>.Error("Güncellemek istediğiniz Eğitim bilgileri getirilirken beklenmeyen bir hata oluştu.");
        }

        var result = await apiResponse.Content.ReadFromJsonAsync<Result<EducationToUpdateDto>>();

        if (!result.IsSuccess)
        {
            string errorMessage;

            if(result.Status == ResultStatus.NotFound)
            {
                errorMessage = "Güncellemek istediğiniz Eğitim bilgisine ulaşılamadı!..";
            }
            errorMessage = "Güncellemek istediğiniz Eğitim bilgileri getirilirken beklenmeyen bir hata oluştu.";

            return Result<EducationToUpdateDto>.Error(errorMessage);
        }

        return Result<EducationToUpdateDto>.Success(result.Value);
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
