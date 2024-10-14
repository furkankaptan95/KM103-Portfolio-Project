using App.DTOs.ExperienceDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IExperienceService
{
    Task<Result<List<AllExperiencesDto>>> GetAllExperiencesAsync();
    Task<Result> AddExperienceAsync(AddExperienceDto dto);
    Task<Result> UpdateExperienceAsync(UpdateExperienceDto dto);
    Task<Result> DeleteExperienceAsync(int id);
    Task<Result> ChangeExperienceVisibilityAsync(int id);
    Task<Result<ExperienceToUpdateDto>> GetByIdAsync(int id);
}
