using App.DTOs.EducationDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IEducationService
{
    Task<Result<List<AllEducationsDto>>> GetAllEducationsAsync();
    Task<Result> AddEducationAsync(AddEducationDto dto);
    Task<Result> UpdateEducationAsync(UpdateEducationDto dto);
    Task<Result> DeleteEducationAsync(int id);
    Task<Result> ChangeEducationVisibilityAsync(int id);
    Task<Result<EducationToUpdateDto>> GetEducationByIdAsync(int id);
}
