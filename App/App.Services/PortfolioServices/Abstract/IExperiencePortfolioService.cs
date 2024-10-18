using App.DTOs.ExperienceDtos.Portfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IExperiencePortfolioService
{
    Task<Result<List<AllExperiencesPortfolioDto>>> GetAllExperiencesAsync();
}
