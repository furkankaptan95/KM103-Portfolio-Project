using App.DTOs.EducationDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IEducationService
{
    Task<Result<List<AllEducationsDto>>> GetAllEducations();
    Task<Result> AddEducation(AddEducationDto dto);
    Task<Result> UpdateEducation(UpdateEducationDto dto);
    Task<Result> DeleteEducation(int id);
    Task<Result> ChangeEducationVisibility(int id);
}
