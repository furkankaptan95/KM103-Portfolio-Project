using App.DTOs.ExperienceDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class ExperienceService(IHttpClientFactory factory) : IExperienceAdminService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddExperienceAsync(AddExperienceDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("add-experience", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Deneyim bilgisi eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage("Deneyim bilgisi başarıyla eklendi.");
        }
        
        catch (Exception)
        {
            return Result.Error("Deneyim bilgisi eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }

    public async Task<Result> ChangeExperienceVisibilityAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"change-experience-visibility-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Deneyimin görünürlüğü başarıyla değiştirildi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Görünürlüğünü değiştirmek istediğiniz Deneyim bulunamadı!..";
            }
            else
            {
                errorMessage = "Deneyimin görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
        
        catch (Exception)
        {
            return Result.Error("Deneyimin görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..");
        }
    }

    public async Task<Result> DeleteExperienceAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.DeleteAsync($"delete-experience-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Deneyim bilgisi başarıyla silindi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Silmek istediğiniz Deneyim bilgisi bulunamadı!..";
            }
            else
            {
                errorMessage = "Deneyim bilgisi silinirken beklenmedik bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
        
        catch (Exception)
        {
            return Result.Error("Deneyim bilgisi silinirken beklenmedik bir hata oluştu..");
        }
    }
    
    public async Task<Result<List<AllExperiencesDto>>> GetAllExperiencesAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("all-experiences");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllExperiencesDto>>.Error("Deneyimler getirilirken beklenmedik bir hata oluştu..");
            }
            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllExperiencesDto>>>();

            if(result is null)
            {
                return Result<List<AllExperiencesDto>>.Error("Deneyimler getirilirken beklenmedik bir hata oluştu..");
            }

            return result;
        }
       
        catch (Exception)
        {
            return Result<List<AllExperiencesDto>>.Error("Deneyimler getirilirken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result<ExperienceToUpdateDto>> GetByIdAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"get-experience-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<ExperienceToUpdateDto>>();

                if (result is null)
                {
                    return Result<ExperienceToUpdateDto>.Error("Güncellemek istediğiniz Deneyim bilgileri getirilirken beklenmeyen bir hata oluştu.");
                }

                return result;
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Güncellemek istediğiniz Deneyim bilgisine ulaşılamadı!..";
            }
            else
            {
                errorMessage = "Güncellemek istediğiniz Deneyim bilgileri getirilirken beklenmeyen bir hata oluştu.";
            }

            return Result<ExperienceToUpdateDto>.Error(errorMessage);
        }
     
        catch (Exception)
        {
            return Result<ExperienceToUpdateDto>.Error("Güncellemek istediğiniz Deneyim bilgileri getirilirken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<Result> UpdateExperienceAsync(UpdateExperienceDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PutAsJsonAsync("update-experience", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Error("Güncellemek istediğiniz Deneyim bilgisi bulunamadı.");
                }

                return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage("Deneyim bilgileri başarıyla güncellendi.");
        }
        
        catch (Exception)
        {
            return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }
}
