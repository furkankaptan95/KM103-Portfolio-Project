using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IAboutMeService
{
    Task<Result> AddAboutMeAsync();
    Task<Result> GetAboutMeAsync();
    Task<Result> UpdateAboutMeAsync();

}
