using App.DTOs.PersonalInfoDtos;
using App.DTOs.PersonalInfoDtos.Admin;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IPersonalInfoAdminService
{
    Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto);
    Task<Result<PersonalInfoAdminDto>> GetPersonalInfoAsync();
    Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto);
    Task<Result<bool>> CheckPersonalInfoAsync();
}
