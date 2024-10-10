using App.ViewModels.AdminMvc.CommentsViewModels;

namespace App.ViewModels.AdminMvc.UsersViewModels;
public class AllUsersViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public List<UsersCommentsViewModel> Comments { get; set; } = new();

}
