using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.PortfolioMvc.UserViewModels;
public class EditUsernameViewModel
{
    public string Email { get; set; }

    [Required(ErrorMessage = "İsim kısmı zorunludur.")]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "İsim sadece boşluk olamaz.")]
    [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
    public string Username { get; set; }
}
