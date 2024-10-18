using App.DTOs.AboutMeDtos;
using App.DTOs.AboutMeDtos.Admin;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IAboutMeAdminService
{
    Task<Result> AddAboutMeAsync(AddAboutMeMVCDto dto);
    Task<Result> AddAboutMeAsync(AddAboutMeApiDto dto);
    Task<Result<AboutMeAdminDto>> GetAboutMeAsync();
    Task<Result> UpdateAboutMeAsync(UpdateAboutMeMVCDto dto);
    Task<Result> UpdateAboutMeAsync(UpdateAboutMeApiDto dto);
}
