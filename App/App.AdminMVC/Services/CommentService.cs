using App.DTOs.CommentDtos.Admin;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class CommentService(IHttpClientFactory factory) : ICommentAdminService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public async Task<Result> ApproveOrNotApproveCommentAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"(not)-approve-comment-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Yorum onay durumu başarıyla değiştirildi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Onay durumunu değiştirmek istediğiniz Yorum bulunamadı!..";
            }
            else
            {
                errorMessage = "Yorumun onay durumu değiştirilirken beklenmeyen bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
        catch (Exception)
        {
            return Result.Error("Yorumun onay durumu değiştirilirken beklenmeyen bir hata oluştu..");
        }
    }
    public async Task<Result> DeleteCommentAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"delete-comment-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Yorum başarıyla silindi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Silmek istediğiniz Yorum bulunamadı!..";
            }
            else
            {
                errorMessage = "Yorum silinirken beklenmedik bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
        catch (Exception)
        {
            return Result.Error("Yorum silinirken beklenmedik bir hata oluştu..");
        }
    }
    public async Task<Result<List<AllCommentsAdminDto>>> GetAllCommentsAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("all-comments");

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result<List<AllCommentsAdminDto>>.Error("Yorumlar getirilirken beklenmedik bir hata oluştu..");
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllCommentsAdminDto>>>();

            if(result is null)
            {
               return Result<List<AllCommentsAdminDto>>.Error("Yorumlar getirilirken beklenmedik bir hata oluştu..");
            }

            return result;
        }
       
        catch (Exception)
        {
            return Result<List<AllCommentsAdminDto>>.Error("Yorumlar getirilirken beklenmedik bir hata oluştu..");
        }
    }
    public Task<Result<List<UsersCommentsDto>>> GetUsersCommentsAsync(int id)
    {
        throw new NotImplementedException();
    }
}
