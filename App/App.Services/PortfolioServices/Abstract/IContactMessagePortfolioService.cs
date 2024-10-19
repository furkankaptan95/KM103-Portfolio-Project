using App.DTOs.ContactMessageDtos.Portfolio;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IContactMessagePortfolioService
{
    Task<Result> AddContactMessageAsync(AddContactMessageDto dto);
}
