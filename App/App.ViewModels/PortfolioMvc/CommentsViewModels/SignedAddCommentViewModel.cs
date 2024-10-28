using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.PortfolioMvc.CommentsViewModels;
public class SignedAddCommentViewModel
{
    [Required(ErrorMessage = "İçerik kısmı zorunludur.")]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "İçerik sadece boşluk olamaz.")]
    [MaxLength(300, ErrorMessage = "İçerik en fazla 300 karakter olabilir.")]
    public string Content { get; set; }
    public int UserId { get; set; }
    public int BlogPostId { get; set; }
}
