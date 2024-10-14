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

    public Task<Result<List<AllExperiencesDto>>> GetAllExperiencesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateExperienceAsync(UpdateExperienceDto dto)
    {
        throw new NotImplementedException();
    }
}
