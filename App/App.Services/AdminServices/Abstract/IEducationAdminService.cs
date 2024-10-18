using App.DTOs.EducationDtos;
using App.DTOs.EducationDtos.Admin;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IEducationAdminService
{
    Task<Result<List<AllEducationsAdminDto>>> GetAllEducationsAsync();
    Task<Result> AddEducationAsync(AddEducationDto dto);
    Task<Result> UpdateEducationAsync(UpdateEducationDto dto);
    Task<Result> DeleteEducationAsync(int id);
    Task<Result> ChangeEducationVisibilityAsync(int id);
    Task<Result<EducationToUpdateDto>> GetEducationByIdAsync(int id);
}
