using App.DTOs.BlogPostDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IBlogPostService
{
    Task<Result<List<AllBlogPostsDto>>> GetAllBlogPostsAsync();
    Task<Result> AddBlogPostAsync(AddBlogPostDto dto);
    Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto);
    Task<Result> DeleteBlogPostAsync(int id);
    Task<Result<BlogPostToUpdateDto>> GetBlogPostById(int id);
    Task<Result> ChangeBlogPostVisibilityAsync(int id);
}
