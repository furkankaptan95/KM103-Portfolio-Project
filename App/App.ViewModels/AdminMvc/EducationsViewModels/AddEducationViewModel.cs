using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.EducationsViewModels;
public class AddEducationViewModel
{
    [Required(ErrorMessage = "Derece kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Derece kısmı en fazla 100 karakter olabilir.")]
    public string Degree { get; set; } = string.Empty;
    [Required(ErrorMessage = "Okul kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Okul kısmı en fazla 100 karakter olabilir.")]
    public string School { get; set; } = string.Empty;
    [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
    public DateTime EndDate { get; set; }

}

