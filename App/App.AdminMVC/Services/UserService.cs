using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class UserService : IUserService
{
    public Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }
}
