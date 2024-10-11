using App.DTOs.ExperienceDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IExperienceService
{
    Task<Result<List<AllExperiencesDto>>> GetAllExperiences();
    Task<Result> AddExperience(AddExperienceDto dto);
    Task<Result> UpdateExperience(UpdateExperienceDto dto);
    Task<Result> DeleteExperience(int id);
    Task<Result> ChangeExperienceVisibility(int id);
}
