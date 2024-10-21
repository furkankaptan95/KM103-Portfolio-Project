using App.ViewModels.AdminMvc.ProjectsViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace App.Core.Validators.DtoValidators.ProjectValidators;
public class AddProjectViewModelValidator : AbstractValidator<AddProjectViewModel>
{
    public AddProjectViewModelValidator()
    {
        
        RuleFor(x => x.ImageFile)
            .NotNull().WithMessage("Image kısmı boş olamaz.")
            .Must(BeAValidImage).WithMessage("Image geçerli bir resim dosyası olmalıdır.");

        RuleFor(x => x.Title)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Başlık kısmı boş olamaz.")
             .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Açıklama kısmı boş olamaz.");
    }
    private bool BeAValidImage(IFormFile file)
    {
        // Resim dosyası uzantılarını kontrol ediyoruz
        if (file == null || file.Length == 0)
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return allowedExtensions.Contains(extension);
    }
}
