using App.DTOs.BlogPostDtos.Admin;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IBlogPostAdminService
{
    Task<Result<List<AllBlogPostsAdminDto>>> GetAllBlogPostsAsync();
    Task<Result> AddBlogPostAsync(AddBlogPostDto dto);
    Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto);
    Task<Result> DeleteBlogPostAsync(int id);
    Task<Result<BlogPostToUpdateDto>> GetBlogPostById(int id);
    Task<Result> ChangeBlogPostVisibilityAsync(int id);
}
