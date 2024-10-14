using App.ViewModels.AdminMvc.EducationsViewModels.Validation;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.EducationsViewModels;
public class UpdateEducationViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Derece kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Derece kısmı en fazla 100 karakter olabilir.")]
    public string Degree { get; set; } = string.Empty;

    [Required(ErrorMessage = "Okul kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Okul kısmı en fazla 100 karakter olabilir.")]
    public string School { get; set; } = string.Empty;

    [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
    [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih giriniz.")]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih giriniz.")]
    [EndDateAfterStartDate]
    public DateTime? EndDate { get; set; }
}
