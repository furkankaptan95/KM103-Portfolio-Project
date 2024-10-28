using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.PortfolioMvc.CommentsViewModels;
public class UnSignedAddCommentViewModel
{
    [Required(ErrorMessage = "İçerik kısmı zorunludur.")]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "İçerik sadece boşluk olamaz.")]
    [MaxLength(300, ErrorMessage = "İçerik en fazla 300 karakter olabilir.")]
    public string Content { get; set; }

    [Required(ErrorMessage = "İsim zorunludur.")]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "İsim sadece boşluk olamaz.")]
    [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
    public string UnsignedCommenterName { get; set; }
    public int BlogPostId { get; set; }
}
