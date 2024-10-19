using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.PortfolioMvc.CommentsViewModels;
public class UnSignedAddCommentViewModel
{
    [Required(ErrorMessage = "İçerik kısmı zorunludur.")]
    public string Content { get; set; }

    [Required(ErrorMessage = "İsim zorunludur.")]
    [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
    public string UnsignedCommenterName { get; set; }
    public int BlogPostId { get; set; }
}
