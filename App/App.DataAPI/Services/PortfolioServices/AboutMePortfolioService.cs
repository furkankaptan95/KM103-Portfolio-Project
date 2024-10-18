using App.DTOs.AboutMeDtos.Porfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services.PortfolioServices;
public class AboutMePortfolioService : IAboutMePortfolioService
{
    public Task<Result<AboutMePortfolioDto>> GetAboutMeAsync()
    {
        throw new NotImplementedException();
    }
}
