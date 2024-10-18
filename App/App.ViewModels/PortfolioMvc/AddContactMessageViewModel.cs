using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.PortfolioMvc;
public class AddContactMessageViewModel
{
    [Required(ErrorMessage = "İsim kısmı zorunludur.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email kısmı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string Email { get; set; }

    public string Subject { get; set; }

    [Required(ErrorMessage = "Mesaj kısmı zorunludur.")]
    public string Message { get; set; }
}
