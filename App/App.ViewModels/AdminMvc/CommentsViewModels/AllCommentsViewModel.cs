namespace App.ViewModels.AdminMvc.CommentsViewModels;
public class AllCommentsViewModel
{
    public int Id { get; set; }
    public string Commenter { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsApproved { get; set; } = false;
    public string BlogPostName { get; set; } = string.Empty;
}
