using App.DTOs.PersonalInfoDtos.Portfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IPersonalInfoPortfolioService
{
    Task<Result<PersonalInfoPortfolioDto>> GetPersonalInfoAsync();
}
