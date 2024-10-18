using App.DTOs.ExperienceDtos.Admin;
using App.DTOs.ExperienceDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class ExperiencePortfolioService : IExperiencePortfolioService
{
    public Task<Result<List<DTOs.ExperienceDtos.Portfolio.AllExperiencesPortfolioDto>>> GetAllExperiencesAsync()
    {
        throw new NotImplementedException();
    }
}
