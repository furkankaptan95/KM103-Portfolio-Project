using App.DTOs.ExperienceDtos;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class UserService(IHttpClientFactory factory) : IUserService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    private HttpClient AuthApiClient => factory.CreateClient("authApi");
    public Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {
        var apiResponse = await AuthApiClient.GetAsync("all-users");

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result<List<AllUsersDto>>.Error("Kullanıcılar getirilirken beklenmedik bir hata oluştu..");
        }

        return await apiResponse.Content.ReadFromJsonAsync<Result<List<AllUsersDto>>>();
    }

    public Task<Result<string>> GetCommentsUserName(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<int>> GetUsersCount(int id)
    {
        throw new NotImplementedException();
    }
}
