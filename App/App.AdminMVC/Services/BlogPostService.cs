using App.DTOs.BlogPostDtos.Admin;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.AdminMVC.Services;
public class BlogPostService : IBlogPostAdminService
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
                return Result.Error("Blog Post eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage("Blog Post başarıyla eklendi.");
        }
        catch (Exception)
        {
            return Result.Error("Blog Post eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
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

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
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
            var apiResponse = await DataApiClient.GetAsync($"delete-blog-post-{id}");

            if (apiResponse.IsSuccessStatusCode)
            {
                return Result.SuccessWithMessage("Blog Post başarıyla silindi.");
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
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
    public async Task<Result<List<AllBlogPostsAdminDto>>> GetAllBlogPostsAsync()
    {
        try
        {
            var apiResponse = await DataApiClient.GetAsync("all-blog-posts");

            if (apiResponse.IsSuccessStatusCode)
            {
                var result = await apiResponse.Content.ReadFromJsonAsync<Result<List<AllBlogPostsAdminDto>>>();

                if(result is null)
                {
                    return Result<List<AllBlogPostsAdminDto>>.Error("Blog Postlar getirilirken beklenmedik bir hata oluştu.");
                }

                return result;
            }
            return Result<List<AllBlogPostsAdminDto>>.Error("Blog Postlar getirilirken beklenmedik bir hata oluştu.");
        }

        catch (Exception)
        {
            return Result<List<AllBlogPostsAdminDto>>.Error("Blog Postlar getirilirken beklenmedik bir hata oluştu.");
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
                    return Result<BlogPostToUpdateDto>.Error("Blog Post verisi alınırken bir hata oluştu.");
                }

                return result;
            }

            string errorMessage;

            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                errorMessage = "Düzenlemek istediğiniz Blog Post bulunamadı!..";
            }
            else
            {
                errorMessage = "Blog Post verisi alınırken bir hata oluştu.";
            }

            return Result<BlogPostToUpdateDto>.Error(errorMessage);
        }
        
        catch (Exception)
        {
            return Result<BlogPostToUpdateDto>.Error("Blog Post verisi alınırken beklenmedik bir hata oluştu.");
        }
    }
    public async Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto)
    {
        try
        {
            var apiResponse = await DataApiClient.PostAsJsonAsync("update-blog-post", dto);

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.NotFound("Güncellemek istediğiniz Blog Post bulunamadı.");
                }

                return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
            }

            return Result.SuccessWithMessage("Blog Post başarıyla güncellendi.");
        }
        catch (Exception)
        {
            return Result.Error("Güncelleme işlemi sırasında beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.");
        }
    }
}
