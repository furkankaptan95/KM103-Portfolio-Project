using App.ViewModels.AdminMvc.AboutMeViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace App.Core.Validators.ViewModelValidators.AboutMeValidators;
public class AddAboutMeViewModelValidator : AbstractValidator<AddAboutMeViewModel>
{
    public AddAboutMeViewModelValidator()
    {
        // Giriş validasyonu
        RuleFor(x => x.Introduction)
            .MaximumLength(100).WithMessage("Giriş maksimum 100 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Giriş kısmı boş olamaz.");

        // Tam isim validasyonu
        RuleFor(x => x.FullName)
            .MaximumLength(100).WithMessage("Tam isim maksimum 100 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Tam isim kısmı boş olamaz.");

        // Alan validasyonu
        RuleFor(x => x.Field)
            .MaximumLength(50).WithMessage("Alan maksimum 50 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Alan kısmı boş olamaz.");

        // Image1 validasyonu
        RuleFor(x => x.Image1)
            .NotNull().WithMessage("Image1 kısmı boş olamaz.")
            .Must(BeAValidImage).WithMessage("Image1 geçerli bir resim dosyası olmalıdır.");

        // Image2 validasyonu
        RuleFor(x => x.Image2)
            .NotNull().WithMessage("Image2 kısmı boş olamaz.")
            .Must(BeAValidImage).WithMessage("Image2 geçerli bir resim dosyası olmalıdır.");
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
