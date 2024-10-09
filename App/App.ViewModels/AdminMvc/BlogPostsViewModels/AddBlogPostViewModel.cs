using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.BlogPostsViewModels;
public class AddBlogPostViewModel
{
    [Required(ErrorMessage = "Başlık kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Başlık kısmı en fazla 100 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "İçerik kısmı zorunludur.")]
    [MaxLength(1500, ErrorMessage = "İçerik kısmı en fazla 1500 karakter olabilir.")]
    public string Content { get; set; } = string.Empty;
}
