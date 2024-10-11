using App.DTOs.PersonalInfoDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IPersonalInfoService
{
    Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto);
    Task<Result<ShowPersonalInfoDto>> GetPersonalInfoAsync();
    Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto);
}
