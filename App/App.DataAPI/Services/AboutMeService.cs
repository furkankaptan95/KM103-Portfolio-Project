using App.Data.DbContexts;
using App.DTOs.AboutMeDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services
{
    public class AboutMeService(DataApiDbContext dataApiDb) : IAboutMeService
    {
        public Task<Result> AddAboutMeAsync(AddAboutMeDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ShowAboutMeDto>> GetAboutMeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAboutMeAsync(UpdateAboutMeDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
