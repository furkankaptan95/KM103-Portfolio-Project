using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class EducationService(IHttpClientFactory factory) : IEducationService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> AddEducationAsync(AddEducationDto dto)
    {
        throw new NotImplementedException();
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

    public Task<Result> UpdateEducationAsync(UpdateEducationDto dto)
    {
        throw new NotImplementedException();
    }
}
