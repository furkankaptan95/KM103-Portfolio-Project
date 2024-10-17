using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

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
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("add-blog-post", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return Result.Error("Blog Post eklenirken beklenmedik bir hata oluştu..");
            }

            return Result.SuccessWithMessage("Blog Post başarıyla eklendi.");
        }
        catch (Exception)
        {
            return Result.Error("Blog Post eklenirken beklenmedik bir hata oluştu..");
        }
    }
    public async Task<Result> ChangeBlogPostVisibilityAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"change-blog-post-visibility-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Blog Post'un görünürlüğü başarıyla değiştirildi.");
            }

            string errorMessage;

            var result = await apiResponse.Content.ReadFromJsonAsync<Result>();

            if (result?.Status == ResultStatus.NotFound)
            {
                errorMessage = "Görünürlüğünü değiştirmek istediğiniz Blog Post bulunamadı!..";
            }
            else
            {
                errorMessage = "Blog Post'un görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }

        catch (Exception)
        {
            return Result.Error("Blog Post'un görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..");
        }
    }
    public async Task<Result> DeleteBlogPostAsync(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.DeleteAsync($"delete-blog-post-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Blog Post başarıyla silindi.");
            }

            string errorMessage;
            var errorResult = await apiResponse.Content.ReadFromJsonAsync<Result>();

            if (errorResult?.Status == ResultStatus.NotFound)
            {
                errorMessage = "Silmek istediğiniz Blog Post bulunamadı!..";
            }

            else
            {
                errorMessage = "Blog Post silinirken beklenmedik bir hata oluştu..";
            }

            return Result.Error(errorMessage);
        }
        catch (Exception)
        {
            return Result.Error("Blog Post silinirken beklenmedik bir hata oluştu..");
        }
    }
    public async Task<Result<List<AllBlogPostsDto>>> GetAllBlogPostsAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("all-blog-posts");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllBlogPostsDto>>>();

                if(result is null)
                {
                    return Result.Error("Blog Postlar getirilirken beklenmedik bir hata oluştu.");
                }

                return result;
            }
            return Result<List<AllBlogPostsDto>>.Error("Blog Postlar getirilirken beklenmedik bir hata oluştu.");
        }

        catch (Exception)
        {
            return Result.Error("Blog Post verisi alınırken beklenmedik bir hata oluştu.");
        }
    }
    public async Task<Result<BlogPostToUpdateDto>> GetBlogPostById(int id)
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync($"blog-post-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<BlogPostToUpdateDto>>();

                if (result is null)
                {
                    return Result.Error("Blog Post verisi alınırken bir hata oluştu.");
                }

                return result;
            }

            string errorMessage;

            var errorResult = await apiResponse.Content.ReadFromJsonAsync<Result<BlogPostToUpdateDto>>();

            if (errorResult?.Status == ResultStatus.NotFound)
            {
                errorMessage = "Düzenlemek istediğiniz Blog Post bulunamadı!..";
            }
            else
            {
                errorMessage = "Blog Post verisi alınırken bir hata oluştu.";
            }

            return Result.Error(errorMessage);
        }
        
        catch (Exception ex)
        {
            return Result.Error("Blog Post verisi alınırken beklenmedik bir hata oluştu.");
        }
    }
    public async Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PutAsJsonAsync("update-blog-post", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Error("Güncellemek istediğiniz Blog Post bulunamadı.");
                }

                return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..");
            }

            return Result.SuccessWithMessage("Blog Post başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..");
        }
    }
}
