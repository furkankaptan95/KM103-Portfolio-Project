using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.PortfolioMvc.CommentsViewModels;
public class SignedAddCommentViewModel
{
    [Required(ErrorMessage = "İçerik kısmı zorunludur.")]
    public string Content { get; set; }
    public int UserId { get; set; }
    public int BlogPostId { get; set; }
}
