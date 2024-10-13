using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.BlogPostsViewModels;
public class UpdateBlogPostViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Başlık kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Başlık kısmı en fazla 100 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "İçerik kısmı zorunludur.")]
    public string Content { get; set; } = string.Empty;
}
