using App.DTOs.BlogPostDtos.Admin;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class BlogPosPortfolioService : IBlogPosPortfolioService
{
    public Task<Result<List<AllBlogPostsAdminDto>>> GetAllBlogPostsAsync()
    {
        throw new NotImplementedException();
    }
}
