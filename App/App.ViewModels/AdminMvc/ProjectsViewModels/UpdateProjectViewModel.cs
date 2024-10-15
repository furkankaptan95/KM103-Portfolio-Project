using App.ViewModels.AdminMvc.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.ProjectsViewModels;
public class UpdateProjectViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Başlık kısmı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Başlık kısmı en fazla 100 karakter olabilir.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Açıklama kısmı zorunludur.")]
    [MaxLength(600, ErrorMessage = "Açıklama kısmı en fazla 600 karakter olabilir.")]
    public string Description { get; set; }

    [ImageFileValidation]
    public IFormFile? ImageFile { get; set; }
    public string? ImageUrl { get; set; }
}
