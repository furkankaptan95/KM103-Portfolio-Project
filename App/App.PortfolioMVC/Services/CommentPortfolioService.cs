using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.PortfolioMVC.Services;
public class CommentPortfolioService(IHttpClientFactory factory) : ICommentPortfolioService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> AddCommentSignedAsync(AddCommentSignedDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("add-comment-signed", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Yorumunuz alınırken beklenmedik bir hata oluştu..");
            }

            return Result.SuccessWithMessage("Yorumunuz başarıyla alınıp yönetici onayına sunuldu.");
        }

        catch (Exception)
        {
            return Result.Error("Yorumunuz alınırken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result> AddCommentUnsignedAsync(AddCommentUnsignedDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("add-comment-unsigned", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Yorumunuz alınırken beklenmedik bir hata oluştu..");
            }

            return Result.SuccessWithMessage("Yorumunuz başarıyla alınıp yönetici onayına sunuldu.");
        }

        catch (Exception)
        {
            return Result.Error("Yorumunuz alınırken beklenmedik bir hata oluştu..");
        }
    }

    public async Task<Result> DeleteCommentAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"delete-comment-portfolio-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Yorumunuz başarıyla silindi.");
            }

            string errorMessage = "Yorumunuz silinirken beklenmeyen bir hata oluştu..";

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Silmek istediğiniz Yorum bulunamadı!..";
            }
            else if (apiResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                errorMessage = "Eğer yorum size aitse silmek için lütfen giriş yapın.";
            }
            else if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
            {
                errorMessage = "Yorumu silmek için yetkiniz bulunmuyor!..";
            }

            return Result.Error(errorMessage);
        }
        catch (Exception)
        {
            return Result.Error("Yorumunuz silinirken beklenmeyen bir hata oluştu..");
        }
    }

    public Task<Result<List<BlogPostCommentsPortfolioDto>>> GetBlogPostCommentsAsync(int id)
	{
		throw new NotImplementedException();
	}
}
