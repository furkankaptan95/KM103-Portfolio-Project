using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class BlogPostService : IBlogPostService
{
    private readonly IHttpClientFactory _factory;
    public BlogPostService(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    private HttpClient DataApiClient => _factory.CreateClient("dataApi");
    public async Task<Result> AddBlogPostAsync(AddBlogPostDto dto)
    {
        var apiResponse = await DataApiClient.PostAsJsonAsync("add-blog-post", dto);

        var result =  await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            return Result.Error("Blog Post eklenirken beklenmeyen bir hata oluştu..");
        }

        return Result.SuccessWithMessage("Blog Post başarıyla eklendi.");

    }

    public async Task<Result> ChangeBlogPostVisibilityAsync(int id)
    {
        var apiResponse = await DataApiClient.GetAsync($"change-blog-post-visibility-{id}");

        var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            string errorMessage;

            if(result.Status == ResultStatus.NotFound)
            {
                errorMessage = "Görünürlüğünü değiştirmek istediğiniz Blog Post bulunamadı!..";
            }

            errorMessage = "Blog Post'un görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            return Result.Error(errorMessage);
        }

        return Result.SuccessWithMessage("Blog Post'un görünürlüğü başarıyla değiştirildi.");
    }

    public async Task<Result> DeleteBlogPostAsync(int id)
    {
        var apiResponses = await DataApiClient.DeleteAsync($"delete-blog-post-{id}");

        var result =  await apiResponses.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            string errorMessage;

            if(result.Status == ResultStatus.NotFound)
            {
                errorMessage = "Silmek istediğiniz Blog Post bulunamadı!..";
            }

            errorMessage = "Blog Post silinirken beklenmedik bir hata oluştu..";

            return Result.Error(errorMessage);
        }

        return Result.SuccessWithMessage("Blog Post başarıyla silindi.");
    }

    public async Task<Result<List<AllBlogPostsDto>>> GetAllBlogPostsAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("all-blog-posts");

        var result =  await apiResponse.Content.ReadFromJsonAsync<Result<List<AllBlogPostsDto>>>();

        if (!result.IsSuccess)
        {
            return Result.Error("Blog Postlar getirilirken beklenmedik bir hata oluştu.");
        }

        return Result.Success(result.Value);
    }

    public async Task<Result<BlogPostToUpdateDto>> GetBlogPostById(int id)
    {
        var apiResponse = await DataApiClient.GetAsync($"blog-post-{id}");

        var result = await apiResponse.Content.ReadFromJsonAsync<Result<BlogPostToUpdateDto>>();

        if (!result.IsSuccess)
        {
            string errorMessage;

            if(result.Status == ResultStatus.NotFound)
            {
                errorMessage = "Düzenlemek istediğiniz Blog Post bulunamadı!..";
            }

            else
            {
                errorMessage = "Düzenlemek istediğiniz Blog Post getirilirken beklenmedik bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }

        return Result.Success(result.Value);
    }

    public async Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto)
    {
        var apiResponse = await DataApiClient.PutAsJsonAsync("update-blog-post", dto);

        var result =  await apiResponse.Content.ReadFromJsonAsync<Result>();

        if (!result.IsSuccess)
        {
            return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..");
        }

        return Result.SuccessWithMessage("Blog Post başarıyla güncellendi.");
    }
}
