namespace App.ViewModels.PortfolioMvc.CommentsViewModels;
public class AddCommentViewModel
{
    public string Content { get; set; }
    public string? UnsignedCommenterName { get; set; }
    public int? UserId { get; set; }
    public int BlogPostId { get; set; }
}
