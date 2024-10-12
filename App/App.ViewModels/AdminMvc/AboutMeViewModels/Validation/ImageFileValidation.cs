using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.AboutMeViewModels.Validation;
public class ImageFileValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        // Dosya yüklenmiş mi kontrolü
        var validTypes = new[] { "image/jpeg", "image/png", "image/gif" };
        if (file != null && !validTypes.Contains(file.ContentType))
        {
            return new ValidationResult(ErrorMessage ?? "Lütfen geçerli bir resim dosyası yükleyiniz."); // Varsayılan mesaj
        }

        return ValidationResult.Success;
    }
}


