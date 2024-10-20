using App.ViewModels.AdminMvc.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.AboutMeViewModels;
public class UpdateAboutMeViewModel
{
    [Required(ErrorMessage = "Giriş kısmı zorunludur.")]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "Giriş sadece boşluk olamaz.")]
    [MaxLength(100, ErrorMessage = "Giriş en fazla 100 karakter olabilir.")]
    public string Introduction { get; set; }

    [Required(ErrorMessage = "Tam isim kısmı zorunludur.")]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "Tam isim sadece boşluk olamaz.")]
    [MaxLength(50, ErrorMessage = "Tam isim en fazla 50 karakter olabilir.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Alan kısmı zorunludur.")]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "Alan sadece boşluk olamaz.")]
    [MaxLength(50, ErrorMessage = "Alan en fazla 50 karakter olabilir.")]
    public string Field { get; set; }
    public string? ImageUrl1 { get; set; }
    public string? ImageUrl2 { get; set; }

    [ImageFileValidation]
    public IFormFile? ImageFile1 { get; set; }

    [ImageFileValidation]
    public IFormFile? ImageFile2 { get; set; }
}
