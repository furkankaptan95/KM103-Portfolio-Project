using App.DTOs.ContactMessageDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class ContactMessagePortfolioService(IHttpClientFactory factory) : IContactMessagePortfolioService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddContactMessageAsync(AddContactMessageDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("add-contact-message", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result.Error("İletişim Formu gönderilirken beklenmedik bir problem oluştu..");
            }

            return Result.SuccessWithMessage("İletişim Formu başarıyla gönderildi.");
        }

        catch (Exception)
        {
            return Result.Error("İletişim Formu gönderilirken beklenmedik bir problem oluştu..");
        }
    }
}
