using App.DTOs.ExperienceDtos.Admin;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.PortfolioServices;
public class ExperiencePortfolioService : IExperiencePortfolioService
{
    public Task<Result<List<AllExperiencesAdminDto>>> GetAllExperiencesAsync()
    {
        throw new NotImplementedException();
    }
}
