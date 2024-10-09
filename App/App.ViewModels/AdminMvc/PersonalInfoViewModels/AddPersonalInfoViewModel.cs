using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.PersonalInfoViewModels;
public class AddPersonalInfoViewModel
{
    [Required(ErrorMessage = "İsim kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "İsim kısmı en fazla 100 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyisim kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Soyisim kısmı en fazla 100 karakter olabilir.")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hakkımda kısmı zorunludur.")]
    [MaxLength(600, ErrorMessage = "Hakkımda kısmı en fazla 600 karakter olabilir.")]
    public string About { get; set; } = string.Empty;

    [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

}
