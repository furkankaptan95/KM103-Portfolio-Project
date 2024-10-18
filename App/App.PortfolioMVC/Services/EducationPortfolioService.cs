using App.DTOs.EducationDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class EducationPortfolioService : IEducationPortfolioService
{
    public Task<Result<List<AllEducationsPortfolioDto>>> GetAllEducationsAsync()
    {
        throw new NotImplementedException();
    }
}
