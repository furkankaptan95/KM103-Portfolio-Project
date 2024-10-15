using App.ViewModels.AdminMvc.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.AboutMeViewModels;
public class UpdateAboutMeViewModel
{
    [Required(ErrorMessage = "Giriş kısmı zorunludur.")]
    [MinLength(10, ErrorMessage = "Giriş kısmı en az 10 karakter olmalıdır.")]
    [MaxLength(1000, ErrorMessage = "Giriş kısmı en fazla 1000 karakter olabilir.")]
    public string Introduction { get; set; }
    public string? ImageUrl1 { get; set; }
    public string? ImageUrl2 { get; set; }

    [ImageFileValidation]
    public IFormFile? ImageFile1 { get; set; }

    [ImageFileValidation]
    public IFormFile? ImageFile2 { get; set; }
}
