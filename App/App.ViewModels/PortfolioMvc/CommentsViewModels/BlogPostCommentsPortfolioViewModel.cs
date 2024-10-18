namespace App.ViewModels.PortfolioMvc.CommentsViewModels;
public class BlogPostCommentsPortfolioViewModel
{
    public int Id { get; set; }
    public string Commenter { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
