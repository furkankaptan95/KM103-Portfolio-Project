using App.DTOs.ProjectDtos.Portfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IProjectPortfolioService
{
    Task<Result<List<AllProjectsPortfolioDto>>> GetAllProjectsAsync();
}
