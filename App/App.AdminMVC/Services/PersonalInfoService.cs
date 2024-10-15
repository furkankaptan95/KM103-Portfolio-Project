using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class PersonalInfoService(IHttpClientFactory factory) : IPersonalInfoService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto)
    {
        var apiResponse = await DataApiClient.PostAsJsonAsync("add-personal-info", dto);

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result.Error("Kişisel Bilgiler eklenirken beklenmedik bir hata oluştu..");
        }

        return Result.SuccessWithMessage("Kişisel Bilgiler başarıyla eklendi.");
    }

    public Task<Result<ShowPersonalInfoDto>> GetPersonalInfoAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto)
    {
        throw new NotImplementedException();
    }
}
