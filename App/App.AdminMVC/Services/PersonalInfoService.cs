using App.DTOs.PersonalInfoDtos;
using App.DTOs.PersonalInfoDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class PersonalInfoService(IHttpClientFactory factory) : IPersonalInfoAdminService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("add-personal-info", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                if(apiResponse.StatusCode == HttpStatusCode.Conflict)
                {
                    return Result.Conflict("Kişisel Bilgiler bölümüne daha önceden zaten ekleme yapılmış!..");
                }

                return Result.Error("Kişisel Bilgiler eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage("Kişisel Bilgiler başarıyla eklendi.");
        }
     
        catch (Exception)
        {
            return Result.Error("Kişisel Bilgiler eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }
    public async Task<Result<bool>> CheckPersonalInfoAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("check-personal-info");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<bool>>();

                if (result is null)
                {
                    return Result<bool>.Error();
                }

                return result;
            }

            return Result<bool>.Error();
        }
        catch (Exception)
        {
            return Result<bool>.Error();
        }
    }

    public async Task<Result<PersonalInfoAdminDto>> GetPersonalInfoAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("get-personal-info");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<PersonalInfoAdminDto>>();
                if(result is null)
                {
                    return Result<PersonalInfoAdminDto>.Error("Bilgiler getirilirken beklenmeyen bir hata oluştu.");
                }
                return result;
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Kişisel bilgiler bölümüne henüz bir şey eklemediniz. Eklemek için gerekli alanları doldurabilirsiniz.";

                return Result<PersonalInfoAdminDto>.NotFound(errorMessage);
            }

            errorMessage = "Bilgiler getirilirken beklenmeyen bir hata oluştu.";

            return Result<PersonalInfoAdminDto>.Error(errorMessage);
        }
      
        catch (Exception)
        {
            return Result<PersonalInfoAdminDto>.Error("Bilgiler getirilirken beklenmeyen bir hata oluştu.");
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

            return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
        
        catch (Exception)
        {
            return Result.Error("Bilgiler güncellenirken beklenmeyen bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }
}
