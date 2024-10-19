using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.BlogPostsViewModels;
public class AddBlogPostViewModel
{
    [Required(ErrorMessage = "Başlık kısmı zorunludur.")]
	[RegularExpression(@"^.*\S.*$", ErrorMessage = "Başlık sadece boşluk olamaz.")]
	[MaxLength(100, ErrorMessage = "Başlık kısmı en fazla 100 karakter olabilir.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "İçerik kısmı zorunludur.")]
	[RegularExpression(@"^.*\S.*$", ErrorMessage = "İçerik sadece boşluk olamaz.")]
	public string Content { get; set; }
}
