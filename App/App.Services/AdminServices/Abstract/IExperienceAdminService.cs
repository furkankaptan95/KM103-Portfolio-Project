using App.DTOs.ExperienceDtos;
using App.DTOs.ExperienceDtos.Admin;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IExperienceAdminService
{
    Task<Result<List<AllExperiencesAdminDto>>> GetAllExperiencesAsync();
    Task<Result> AddExperienceAsync(AddExperienceDto dto);
    Task<Result> UpdateExperienceAsync(UpdateExperienceDto dto);
    Task<Result> DeleteExperienceAsync(int id);
    Task<Result> ChangeExperienceVisibilityAsync(int id);
    Task<Result<ExperienceToUpdateDto>> GetByIdAsync(int id);
}
