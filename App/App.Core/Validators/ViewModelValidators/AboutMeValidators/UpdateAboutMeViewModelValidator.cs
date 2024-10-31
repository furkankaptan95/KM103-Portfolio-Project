using App.ViewModels.AdminMvc.AboutMeViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Core.Validators.ViewModelValidators.AboutMeValidators;
public class UpdateAboutMeViewModelValidator : AbstractValidator<UpdateAboutMeViewModel>
{
    public UpdateAboutMeViewModelValidator()
    {
        RuleFor(x => x.ImageFile1)
            .Must(BeAValidImage).WithMessage("Lütfen geçerli bir resim dosyası yükleyiniz.");

        RuleFor(x => x.ImageFile2)
            .Must(BeAValidImage).WithMessage("Lütfen geçerli bir resim dosyası yükleyiniz.");

        RuleFor(x => x.Introduction)
            .NotEmpty().WithMessage("Giriş kısmı boş olamaz.")
            .MaximumLength(100).WithMessage("Giriş maksimum 100 karakter olabilir.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Tam isim kısmı boş olamaz.")
            .MaximumLength(50).WithMessage("Tam isim maksimum 50 karakter olabilir.");

        RuleFor(x => x.Field)
            .NotEmpty().WithMessage("Alan kısmı boş olamaz.")
            .MaximumLength(50).WithMessage("Alan maksimum 50 karakter olabilir.");
    }
    private bool BeAValidImage(IFormFile? file)
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
