namespace App.ViewModels.AdminMvc.CommentsViewModels;
public class UsersCommentsViewModel
{
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsApproved { get; set; } = false;
    public string BlogPostName { get; set; } = string.Empty;
}
