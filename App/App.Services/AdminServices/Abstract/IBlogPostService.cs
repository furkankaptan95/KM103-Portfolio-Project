using App.DTOs.BlogPostDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IBlogPostService
{
    Task<Result<List<AllBlogPostsDto>>> GetAllBlogPosts();
    Task<Result> AddBlogPost(AddBlogPostDto dto);
    Task<Result> UpdateBlogPost(UpdateBlogPostDto dto);
    Task<Result> DeleteBlogPost(int id);
    Task<Result> ChangeBlogPostVisibility(int id);
}
