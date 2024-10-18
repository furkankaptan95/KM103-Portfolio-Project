using App.DTOs.BlogPostDtos.Admin;
using Ardalis.Result;

namespace App.Services.PortfolioServices.Abstract;
public interface IBlogPosPortfolioService
{
    Task<Result<List<AllBlogPostsAdminDto>>> GetAllBlogPostsAsync();
}
