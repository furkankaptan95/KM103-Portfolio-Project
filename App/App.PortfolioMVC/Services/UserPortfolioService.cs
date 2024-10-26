using App.DTOs.UserDtos;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.PortfolioMVC.Services;
public class UserPortfolioService(IHttpClientFactory factory) : IUserPortfolioService
{
    private HttpClient AuthApiClient => factory.CreateClient("authApi");
    public async Task<Result> EditUsernameAsync(EditUsernameDto dto)
    {
        try
        {
            var response = await AuthApiClient.PostAsJsonAsync("edit-username", dto);

            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Result.Error("Bu Kullanıcı Adı daha önce zaten alınmış!..");
                }

                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Error("Kullanıcı bulunamadı!..");
                }

                return Result.Error("Kullanıcı isminiz değiştirilirken beklenmedik bir hata oluştu!..");
            }


            return Result.SuccessWithMessage("Kullanıcı isminiz başarıyla güncellendi.");
        }
        catch (Exception)
        {
            return Result.Error("Kullanıcı isminiz değiştirilirken beklenmedik bir hata oluştu!..");
        }
    }
}
