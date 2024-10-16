using App.ViewModels.AdminMvc.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.AboutMeViewModels;
public class AddAboutMeViewModel
{
    [Required(ErrorMessage = "Giriş kısmı zorunludur.")]
    public string Introduction { get; set; }

    [Required(ErrorMessage = "1. Fotoğraf zorunludur.")]
    [ImageFileValidation]
    public IFormFile Image1 { get; set; }

    [Required(ErrorMessage = "2. Fotoğraf zorunludur.")]
    [ImageFileValidation]
    public IFormFile Image2 { get; set; }

}
