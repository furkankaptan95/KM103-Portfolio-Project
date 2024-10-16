using App.Data.DbContexts;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services;
public class UserService : IUserService
{
    private readonly IHttpClientFactory _factory;
    public UserService( IHttpClientFactory factory)
    {
        _factory = factory;
    }
    private HttpClient AuthApiClient => _factory.CreateClient("authApi");

    public Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> GetCommentsUserName(int id)
    {
        var apiAuthResponse = await AuthApiClient.GetAsync($"get-commenter-username-{id}");

        if (apiAuthResponse.IsSuccessStatusCode)
        {
            return await apiAuthResponse.Content.ReadFromJsonAsync<Result<string>>();
        }

        return Result.Error();
    }
}
