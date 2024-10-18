using App.ViewModels.PortfolioMvc.CommentsViewModels;

namespace App.ViewModels.PortfolioMvc.BlogPostsViewModels;
public class PortfolioSingleBlogPostsViewModel
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public List<PortfolioBlogPostCommentsViewModel>? Comments { get; set; }
}
