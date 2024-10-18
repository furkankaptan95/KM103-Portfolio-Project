using App.DTOs.AboutMeDtos.Porfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IAboutMePortfolioService
{
    Task<Result<AboutMePortfolioDto>> GetAboutMeAsync();
}
