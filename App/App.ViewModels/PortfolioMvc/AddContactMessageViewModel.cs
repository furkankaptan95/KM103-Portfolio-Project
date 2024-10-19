using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.PortfolioMvc;
public class AddContactMessageViewModel
{
    [Required(ErrorMessage = "İsim kısmı zorunludur.")]
    [RegularExpression(@"\S+", ErrorMessage = "İsim kısmı boşluk olamaz.")]
    [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email kısmı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    [MaxLength(100, ErrorMessage = "Email en fazla 100 karakter olabilir.")]
    public string Email { get; set; }

    [MaxLength(100, ErrorMessage = "Konu en fazla 100 karakter olabilir.")]
    public string? Subject { get; set; }

    [Required(ErrorMessage = "Mesaj kısmı zorunludur.")]
    [RegularExpression(@"\S+", ErrorMessage = "Mesaj kısmı boşluk olamaz.")]
    public string Message { get; set; }
}
