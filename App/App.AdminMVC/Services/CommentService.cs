using App.DTOs.CommentDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class CommentService(IHttpClientFactory factory) : ICommentService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> ApproveOrNotApproveCommentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteCommentAsync(int id)
    {
        throw new NotImplementedException();
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
