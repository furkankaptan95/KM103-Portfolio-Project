using App.DTOs.ExperienceDtos.Admin;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IExperiencePortfolioService
{
    Task<Result<List<AllExperiencesAdminDto>>> GetAllExperiencesAsync();
}
