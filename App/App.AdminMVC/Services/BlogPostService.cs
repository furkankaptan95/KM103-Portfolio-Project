using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class BlogPostService(IHttpClientFactory factory) : IBlogPostService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> AddBlogPostAsync(AddBlogPostDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ChangeBlogPostVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteBlogPostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllBlogPostsDto>>> GetAllBlogPostsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto)
    {
        throw new NotImplementedException();
    }
}
