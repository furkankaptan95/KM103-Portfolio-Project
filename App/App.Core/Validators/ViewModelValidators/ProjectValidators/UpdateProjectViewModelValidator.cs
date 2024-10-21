using App.ViewModels.AdminMvc.ProjectsViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Core.Validators.DtoValidators.ProjectValidators;
public class UpdateProjectViewModelValidator : AbstractValidator<UpdateProjectViewModel>
{
    public UpdateProjectViewModelValidator()
    {
        RuleFor(x => x.ImageFile)
           .Must(BeAValidImage).WithMessage("Image geçerli bir resim dosyası olmalıdır.");

        RuleFor(x => x.Title)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Başlık kısmı boş olamaz.")
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Açıklama kısmı boş olamaz.");
    }
    private bool BeAValidImage(IFormFile file)
    {
        // Eğer dosya yoksa geçerli kabul et
        if (file == null || file.Length == 0)
            return true; // Null veya boş ise geçerli kabul et

        // Resim dosyası uzantılarını kontrol ediyoruz
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return allowedExtensions.Contains(extension);
    }
}
