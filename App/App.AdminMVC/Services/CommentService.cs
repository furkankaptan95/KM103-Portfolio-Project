using App.DTOs.CommentDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class CommentService(IHttpClientFactory factory) : ICommentService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> ApproveOrNotApproveCommentAsync(int id)
    {
        var apiResponse = await DataApiClient.GetAsync($"(not)-approve-comment-{id}");

        if (apiResponse.IsSuccessStatusCode)
        {
            return Result.SuccessWithMessage("Yorum onay durumu başarıyla değiştirildi.");
        }

        string errorMessage;
        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (result.Status == ResultStatus.NotFound)
        {
            errorMessage = "Onay durumunu değiştirmek istediğiniz Yorum bulunamadı!..";
        }
        else
        {
            errorMessage = "Yorumun onay durumu değiştirilirken beklenmeyen bir hata oluştu..";
        }

        return Result.Error(errorMessage);
    }

    public async Task<Result> DeleteCommentAsync(int id)
    {
        var apiResponse = await DataApiClient.DeleteAsync($"delete-comment-{id}");

        if (apiResponse.IsSuccessStatusCode)
        {
            return Result.SuccessWithMessage("Yorum başarıyla silindi.");
        }

        string errorMessage;
        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (result.Status == ResultStatus.NotFound)
        {
            errorMessage = "Silmek istediğiniz Yorum bulunamadı!..";
        }
        else
        {
            errorMessage = "Yorum silinirken beklenmedik bir hata oluştu..";
        }

        return Result.Error(errorMessage);
    }

    public async Task<Result<List<AllCommentsDto>>> GetAllCommentsAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("all-comments");

        if (!apiResponse.IsSuccessStatusCode)
        {
            return Result<List<AllCommentsDto>>.Error("Yorumlar getirilirken beklenmedik bir hata oluştu..");
        }

        return await apiResponse.Content.ReadFromJsonAsync<Result<List<AllCommentsDto>>>();
    }
}
