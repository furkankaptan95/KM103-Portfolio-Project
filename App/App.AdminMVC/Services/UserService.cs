using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class UserService(IHttpClientFactory factory) : IUserService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    private HttpClient AuthApiClient => factory.CreateClient("authApi");
    public async Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        try
        {
            var apiResponse = await AuthApiClient.GetAsync($"change-user-activeness-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Kullanıcının aktifliği başarıyla değiştirildi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Aktifliğini değiştirmek istediğiniz Kullanıcı bulunamadı!..";
            }
            else
            {
                errorMessage = "Kullanıcının aktifliği değiştirilirken beklenmeyen bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
       
        catch (Exception)
        {
            return Result.Error("Kullanıcının aktifliği değiştirilirken beklenmeyen bir hata oluştu..");
        }
    }

    public async Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {
        try
        {
            var apiResponse = await AuthApiClient.GetAsync("all-users");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllUsersDto>>.Error("Kullanıcılar getirilirken beklenmedik bir hata oluştu..");
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllUsersDto>>>();
            if (result is null)
            {
                return Result.Error("Kullanıcılar getirilirken beklenmedik bir hata oluştu..");
            }
                return result;
        }

        catch (Exception)
        {
            return Result.Error("Kullanıcılar getirilirken beklenmedik bir hata oluştu..");
        }
    }

    public Task<Result<string>> GetCommentsUserName(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<int>> GetUsersCount()
    {
        throw new NotImplementedException();
    }
}
