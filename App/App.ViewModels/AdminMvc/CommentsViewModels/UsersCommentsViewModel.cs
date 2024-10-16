namespace App.ViewModels.AdminMvc.CommentsViewModels;
public class UsersCommentsViewModel
{
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsApproved { get; set; }
    public string BlogPostName { get; set; }
}
