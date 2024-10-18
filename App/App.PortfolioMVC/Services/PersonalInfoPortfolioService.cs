using App.DTOs.PersonalInfoDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class PersonalInfoPortfolioService : IPersonalInfoPortfolioService
{
    public Task<Result<PersonalInfoPortfolioDto>> GetPersonalInfoAsync()
    {
        throw new NotImplementedException();
    }
}
