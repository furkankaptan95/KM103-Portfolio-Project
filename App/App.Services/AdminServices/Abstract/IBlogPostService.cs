using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IBlogPostService
{
    Task<Result> GetAllBlogPosts();
    Task<Result> AddBlogPost();
    Task<Result> UpdateBlogPost();
    Task<Result> DeleteBlogPost();
    Task<Result> ChangeBlogPostVisibility();
}
