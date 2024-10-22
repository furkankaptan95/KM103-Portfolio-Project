using App.ViewModels.AdminMvc.AboutMeViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Core.Validators.ViewModelValidators.AboutMeValidators;
public class UpdateAboutMeViewModelValidator : AbstractValidator<UpdateAboutMeViewModel>
{
    public UpdateAboutMeViewModelValidator()
    {
        // Image1 validasyonu
        RuleFor(x => x.ImageFile1)
            .Must(BeAValidImage).WithMessage("Lütfen geçerli bir resim dosyası yükleyiniz.");

        // Image2 validasyonu
        RuleFor(x => x.ImageFile2)
            .Must(BeAValidImage).WithMessage("Lütfen geçerli bir resim dosyası yükleyiniz.");

        // Giriş validasyonu
        RuleFor(x => x.Introduction)
            .MaximumLength(100).WithMessage("Giriş maksimum 100 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Giriş kısmı boş olamaz.");

        // Tam isim validasyonu
        RuleFor(x => x.FullName)
            .MaximumLength(50).WithMessage("Tam isim maksimum 50 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Tam isim kısmı boş olamaz.");

        // Alan validasyonu
        RuleFor(x => x.Field)
            .MaximumLength(50).WithMessage("Alan maksimum 50 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Alan kısmı boş olamaz.");
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
