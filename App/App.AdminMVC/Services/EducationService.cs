using App.DTOs.EducationDtos;
using App.DTOs.EducationDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class EducationService(IHttpClientFactory factory) : IEducationAdminService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddEducationAsync(AddEducationDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("add-education", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Eğitim bilgisi eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage("Eğitim bilgisi başarıyla eklendi.");
        }
       
        catch (Exception)
        {
            return Result.Error("Eğitim bilgisi eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }

    public async Task<Result> ChangeEducationVisibilityAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"change-education-visibility-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Eğitim'in görünürlüğü başarıyla değiştirildi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Görünürlüğünü değiştirmek istediğiniz Eğitim bulunamadı!..";
            }
            else
            {
                errorMessage = "Eğitim'in görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
       
        catch (Exception)
        {
            return Result.Error("Eğitim'in görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..");
        }
    }

    public async Task<Result> DeleteEducationAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"delete-education-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Eğitim bilgisi başarıyla silindi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Silmek istediğiniz Eğitim bilgisi bulunamadı!..";
            }
            else
            {
                errorMessage = "Eğitim bilgisi silinirken beklenmedik bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
       
        catch (Exception)
        {
            return Result.Error("Eğitim bilgisi silinirken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result<List<AllEducationsAdminDto>>> GetAllEducationsAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("all-educations");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllEducationsAdminDto>>.Error("Eğitimler getirilirken beklenmedik bir hata oluştu..");
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllEducationsAdminDto>>>();

            if(result is null)
            {
                return Result<List<AllEducationsAdminDto>>.Error("Eğitimler getirilirken beklenmedik bir hata oluştu..");
            }

            return result;
        }
       
        catch (Exception)
        {
            return Result<List<AllEducationsAdminDto>>.Error("Eğitimler getirilirken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result<EducationToUpdateDto>> GetEducationByIdAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"get-education-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<EducationToUpdateDto>>();

                if(result is null)
                {
                    return Result<EducationToUpdateDto>.Error("Güncellemek istediğiniz Eğitim bilgileri getirilirken beklenmeyen bir hata oluştu.");
                }
                return result;
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Güncellemek istediğiniz Eğitim bilgisine ulaşılamadı!..";
            }
            else
            {
                errorMessage = "Güncellemek istediğiniz Eğitim bilgileri getirilirken beklenmeyen bir hata oluştu.";
            }

            return Result<EducationToUpdateDto>.Error(errorMessage);
        }
        
        catch (Exception)
        {
            return Result<EducationToUpdateDto>.Error("Güncellemek istediğiniz Eğitim bilgileri getirilirken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<Result> UpdateEducationAsync(UpdateEducationDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("update-education", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Error("Güncellemek istediğiniz Eğitim bilgisi bulunamadı.");
                }

                return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage("Eğitim bilgileri başarıyla güncellendi.");
        }
        
        catch (Exception)
        {
            return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }
}
