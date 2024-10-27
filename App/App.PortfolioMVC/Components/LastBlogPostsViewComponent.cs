using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc.BlogPostsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Components;
public class LastBlogPostsViewComponent : ViewComponent
{
    private readonly IBlogPostPortfolioService _blogPostPortfolioService;

    public LastBlogPostsViewComponent(IBlogPostPortfolioService blogPostPortfolioService)
    {
        _blogPostPortfolioService = blogPostPortfolioService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await _blogPostPortfolioService.GetHomeBlogPostsAsync();

        var posts = result.Value;

        var latestPosts = posts
            .OrderByDescending(post => post.PublishDate)
            .Take(3)
            .ToList();

        var models = new List<HomeBlogPostsPortfolioViewModel>();

        foreach (var blogPost in latestPosts)
        {
            var blogPostToAdd = new HomeBlogPostsPortfolioViewModel
            {
                Title = blogPost.Title,
                Content = blogPost.Content,
                Id = blogPost.Id,
                PublishDate = blogPost.PublishDate,
                CommentsCount = blogPost.CommentsCount
            };

            models.Add(blogPostToAdd);
        }

        return View(models);
    }
}
