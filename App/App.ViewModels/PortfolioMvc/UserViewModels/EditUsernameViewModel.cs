using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.PortfolioMvc.UserViewModels;
public class EditUsernameViewModel
{
    public string Email { get; set; }

    [Required(ErrorMessage = "İsim kısmı zorunludur.")]
    [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
    [RegularExpression(@"^\S+$", ErrorMessage = "İsim boşluk içeremez.")]
    public string Username { get; set; }
}
