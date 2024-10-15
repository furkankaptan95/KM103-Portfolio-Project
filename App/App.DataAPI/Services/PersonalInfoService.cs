using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services;
public class PersonalInfoService : IPersonalInfoService
{
    public Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ShowPersonalInfoDto>> GetPersonalInfoAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto)
    {
        throw new NotImplementedException();
    }
}
