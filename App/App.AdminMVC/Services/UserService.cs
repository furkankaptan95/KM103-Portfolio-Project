using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class UserService(IHttpClientFactory factory) : IUserService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }
}
