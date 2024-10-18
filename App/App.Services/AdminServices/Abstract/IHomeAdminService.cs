using App.DTOs.HomeDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IHomeAdminService
{
    Task<Result<HomeDto>> GetHomeInfosAsync();
}
