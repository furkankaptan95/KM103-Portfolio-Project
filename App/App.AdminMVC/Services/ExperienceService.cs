using App.DTOs.EducationDtos;
using App.DTOs.ExperienceDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class ExperienceService(IHttpClientFactory factory) : IExperienceService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddExperienceAsync(AddExperienceDto dto)
    {
        var apiResponse = await DataApiClient.PostAsJsonAsync("add-experience", dto);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Deneyim bilgisi eklenirken beklenmedik bir hata oluştu..");
        }

        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            return Result.Error("Deneyim bilgisi eklenirken beklenmedik bir hata oluştu..");
        }

        return Result.SuccessWithMessage("Deneyim bilgisi başarıyla eklendi.");
    }

    public Task<Result> ChangeExperienceVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteExperienceAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result<List<AllExperiencesDto>>> GetAllExperiencesAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("all-experiences");

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result<List<AllExperiencesDto>>.Error("Deneyimler getirilirken beklenmedik bir hata oluştu..");
        }

        var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllExperiencesDto>>>();

        if (!result.IsSuccess)
        {
            return Result<List<AllExperiencesDto>>.Error("Deneyimler getirilirken beklenmedik bir hata oluştu..");
        }

        return Result<List<AllExperiencesDto>>.Success(result.Value);
    }

    public Task<Result<ExperienceToUpdateDto>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateExperienceAsync(UpdateExperienceDto dto)
    {
        var apiResponse = await DataApiClient.PutAsJsonAsync("update-experience", dto);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..");
        }

        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..");
        }

        return Result.SuccessWithMessage("Deneyim bilgileri başarıyla güncellendi.");
    }
}
