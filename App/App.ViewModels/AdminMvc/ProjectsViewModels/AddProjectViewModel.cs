using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.ProjectsViewModels;
public class AddProjectViewModel
{
    [Required(ErrorMessage = "Başlık kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Başlık kısmı en fazla 100 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Açıklama kısmı zorunludur.")]
    [MaxLength(600, ErrorMessage = "Açıklama kısmı en fazla 600 karakter olabilir.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Resim dosyası zorunludur.")]
    [FileExtensions(Extensions = "jpg,jpeg,png,gif", ErrorMessage = "Lütfen geçerli bir resim dosyası yükleyiniz.")]
    public IFormFile ImageFile { get; set; }

}
