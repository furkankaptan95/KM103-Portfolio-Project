namespace App.ViewModels.PortfolioMvc.BlogPostsViewModels;
public class AllBlogPostsPortfolioViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public int CommentsCount { get; set; }
}
