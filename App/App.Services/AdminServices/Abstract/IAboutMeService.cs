using App.DTOs.AboutMeDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IAboutMeService
{
    Task<Result<AddAboutMeDto>> AddAboutMeAsync();
    Task<Result<ShowAboutMeDto>> GetAboutMeAsync();
    Task<Result<UpdateAboutMeDto>> UpdateAboutMeAsync();
}
