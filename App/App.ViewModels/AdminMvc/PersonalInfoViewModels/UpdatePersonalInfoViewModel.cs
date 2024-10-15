using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.PersonalInfoViewModels;
public class UpdatePersonalInfoViewModel
{
    [Required(ErrorMessage = "İsim kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "İsim kısmı en fazla 50 karakter olabilir.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Soyisim kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Soyisim kısmı en fazla 50 karakter olabilir.")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "Hakkımda kısmı zorunludur.")]
    [MaxLength(600, ErrorMessage = "Hakkımda kısmı en fazla 300 karakter olabilir.")]
    public string About { get; set; }

    [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
    [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih giriniz.")]
    public DateTime BirthDate { get; set; }
}
