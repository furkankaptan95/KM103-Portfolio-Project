using App.DTOs.HomeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class HomeService : IHomeService
{
    public Task<Result<HomeDto>> GetHomeInfosAsync()
    {
        throw new NotImplementedException();
    }
}
