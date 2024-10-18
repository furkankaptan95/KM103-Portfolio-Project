using App.DTOs.EducationDtos.Portfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IEducationPortfolioService
{
    Task<Result<List<AllEducationsPortfolioDto>>> GetAllEducationsAsync();
}
