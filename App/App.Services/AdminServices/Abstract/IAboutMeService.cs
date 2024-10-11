using App.DTOs.AboutMeDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IAboutMeService
{
    Task<Result> AddAboutMeAsync(AddAboutMeDto dto);
    Task<Result<ShowAboutMeDto>> GetAboutMeAsync();
    Task<Result> UpdateAboutMeAsync(UpdateAboutMeDto dto);
}
