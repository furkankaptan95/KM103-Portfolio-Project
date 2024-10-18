using App.DTOs.ProjectDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services.PortfolioServices;
public class ProjectPortfolioService : IProjectPortfolioService
{
    public Task<Result<List<AllProjectsPortfolioDto>>> GetAllProjectsAsync()
    {
        throw new NotImplementedException();
    }
}
