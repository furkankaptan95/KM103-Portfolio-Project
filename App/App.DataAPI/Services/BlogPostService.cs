using App.Data.DbContexts;
using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services;
public class BlogPostService(DataApiDbContext dataApiDb) : IBlogPostService
{
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
