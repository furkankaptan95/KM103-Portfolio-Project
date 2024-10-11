using App.DTOs.HomeDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IHomeService
{
    Task<Result<HomeDto>> GetHomeInfosAsync();
}
