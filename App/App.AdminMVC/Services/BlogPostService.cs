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

        return await apiResponse.Content.ReadFromJsonAsync<Result>();
    }

    public Task<Result> ChangeBlogPostVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteBlogPostAsync(int id)
    {
        var apiResponses = await DataApiClient.DeleteAsync($"delete-blog-post-{id}");

        return await apiResponses.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<Result<List<AllBlogPostsDto>>> GetAllBlogPostsAsync()
    {
        var apiResponse = await DataApiClient.GetAsync("all-blog-posts");

        return await apiResponse.Content.ReadFromJsonAsync<Result<List<AllBlogPostsDto>>>();

    }

    public async Task<Result<BlogPostToUpdateDto>> GetBlogPostById(int id)
    {
        var apiResponse = await DataApiClient.GetAsync($"blog-post-{id}");

        return await apiResponse.Content.ReadFromJsonAsync<Result<BlogPostToUpdateDto>>();
    }

    public async Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto)
    {
        var apiResponse = await DataApiClient.PutAsJsonAsync("update-blog-post", dto);

        return await apiResponse.Content.ReadFromJsonAsync<Result>();
    }
}
