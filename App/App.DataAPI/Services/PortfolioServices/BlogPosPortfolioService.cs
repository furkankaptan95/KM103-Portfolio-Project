using App.DTOs.BlogPostDtos.Admin;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.DataAPI.Services.PortfolioServices;
public class BlogPosPortfolioService : IBlogPosPortfolioService
{
    public Task<Result<List<AllBlogPostsAdminDto>>> GetAllBlogPostsAsync()
    {
        throw new NotImplementedException();
    }
}
