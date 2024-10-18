using App.ViewModels.PortfolioMvc;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IHomePortfolioService
{
	Task<Result<HomeIndexViewModel>> GetHomeInfosAsync();
}
