using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class PersonalInfoService(IHttpClientFactory factory) : IPersonalInfoService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("add-personal-info", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Kişisel Bilgiler eklenirken beklenmedik bir hata oluştu..");
            }

            return Result.SuccessWithMessage("Kişisel Bilgiler başarıyla eklendi.");
        }
     
        catch (Exception)
        {
            return Result.Error("Kişisel Bilgiler eklenirken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result<ShowPersonalInfoDto>> GetPersonalInfoAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("get-personal-info");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<ShowPersonalInfoDto>>();
                if(result is null)
                {
                    return Result<ShowPersonalInfoDto>.Error("Bilgiler getirilirken beklenmeyen bir hata oluştu.");
                }
                return result;
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Kişisel bilgiler bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurabilirsiniz.";

                return Result<ShowPersonalInfoDto>.NotFound(errorMessage);
            }

            errorMessage = "Bilgiler getirilirken beklenmeyen bir hata oluştu.";

            return Result<ShowPersonalInfoDto>.Error(errorMessage);
        }
      
        catch (Exception)
        {
            return Result<ShowPersonalInfoDto>.Error("Bilgiler getirilirken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PutAsJsonAsync("update-personal-info", dto);

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage(" Kişisel Bilgileriniz başarılı bir şekilde güncellendi. ");
            }

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.NotFound("Güncellemek istediğiniz Kişisel Bilgiler kısmında herhangi bir bilgi bulunmuyor!..Eklemek için formu doldurabilirsiniz..");
            }

            return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu.. Tekrar güncellemeyi deneyebilirsiniz.");
        }
        
        catch (Exception)
        {
            return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu.. Tekrar güncellemeyi deneyebilirsiniz.");
        }
    }
}
