using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.AboutMeViewModels;
public class AddAboutMeViewModel
{
    [Required(ErrorMessage = "Giriş kısmı zorunludur.")]
    [MinLength(10, ErrorMessage = "Giriş kısmı en az 10 karakter olmalıdır.")]
    public string Introduction { get; set; }

    [Required(ErrorMessage = "1. Fotoğraf zorunludur.")]
    [FileExtensions(Extensions = "jpg,jpeg,png,gif", ErrorMessage = "Lütfen geçerli bir resim dosyası yükleyiniz.")]
    public IFormFile Image1 { get; set; }

    [Required(ErrorMessage = "2. Fotoğraf zorunludur.")]
    [FileExtensions(Extensions = "jpg,jpeg,png,gif", ErrorMessage = "Lütfen geçerli bir resim dosyası yükleyiniz.")]
    public IFormFile Image2 { get; set; }

}
