using App.ViewModels.AdminMvc.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.AboutMeViewModels;
public class AddAboutMeViewModel
{
    [Required(ErrorMessage = "Giriş kısmı zorunludur.")]
    [MinLength(10, ErrorMessage = "Giriş kısmı en az 10 karakter olmalıdır.")]
    [MaxLength(1000, ErrorMessage = "Giriş kısmı en fazla 1000 karakter olabilir.")]
    public string Introduction { get; set; }

    [Required(ErrorMessage = "1. Fotoğraf zorunludur.")]
    [ImageFileValidation]
    public IFormFile Image1 { get; set; }

    [Required(ErrorMessage = "2. Fotoğraf zorunludur.")]
    [ImageFileValidation]
    public IFormFile Image2 { get; set; }

}
