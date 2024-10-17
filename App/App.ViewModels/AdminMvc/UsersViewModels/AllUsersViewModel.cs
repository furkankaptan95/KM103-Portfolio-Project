using App.ViewModels.AdminMvc.CommentsViewModels;

namespace App.ViewModels.AdminMvc.UsersViewModels;
public class AllUsersViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string ImageUrl { get; set; }
    public List<UsersCommentsViewModel> Comments { get; set; }

}
