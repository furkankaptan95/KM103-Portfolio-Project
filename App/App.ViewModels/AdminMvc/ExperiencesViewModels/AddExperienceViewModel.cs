using App.ViewModels.AdminMvc.ExperiencesViewModels.Validation;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.ExperiencesViewModels;
public class AddExperienceViewModel
{
    [Required(ErrorMessage = "Başlık kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Başlık kısmı en fazla 100 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Firma kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Firma kısmı en fazla 100 karakter olabilir.")]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "Açıklama kısmı zorunludur.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
    [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih giriniz.")]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih giriniz.")]
    [EndDateAfterStartDate]
    public DateTime? EndDate { get; set; }
}
