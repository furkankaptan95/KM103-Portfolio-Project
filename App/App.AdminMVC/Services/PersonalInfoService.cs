using App.DTOs.AboutMeDtos;
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

    public async Task<Result<ShowPersonalInfoDto>> GetPersonalInfoAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("get-personal-info");

        if (apiResponse.IsSuccessStatusCode)
        {
            return await apiResponse.Content.ReadFromJsonAsync<Result<ShowPersonalInfoDto>>();
        }

        var result = await apiResponse.Content.ReadFromJsonAsync<Result<ShowPersonalInfoDto>>();

        string errorMessage;

        if (result.Status == ResultStatus.NotFound)
        {
            errorMessage = "Kişisel bilgiler bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurabilirsiniz.";

            return Result<ShowPersonalInfoDto>.NotFound(errorMessage);
        }

        errorMessage = "Bilgiler getirilirken beklenmeyen bir hata oluştu.";

        return Result<ShowPersonalInfoDto>.Error(errorMessage);
    }

    public async Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto)
    {
        var apiResponse = await DataApiClient.PutAsJsonAsync("update-personal-info", dto);

        if (apiResponse.IsSuccessStatusCode)
        {
            return Result.SuccessWithMessage(" -Hakkımda- bilgileriniz başarılı bir şekilde güncellendi. ");
        }

        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if(result.Status == ResultStatus.NotFound)
        {
            return Result.NotFound("Güncellemek istediğiniz Kişisel Bilgiler kısmında herhangi bir bilgi bulunmuyor!..Eklemek için formu doldurabilirsiniz..");
        }

        return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu.. Tekrar güncellemeyi deneyebilirsiniz.");

    }
}
