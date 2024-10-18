using App.ViewModels.PortfolioMvc.CommentsViewModels;

namespace App.ViewModels.PortfolioMvc.BlogPostsViewModels;
public class SingleBlogPostViewModel
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public List<BlogPostCommentsPortfolioViewModel>? Comments { get; set; }
}
