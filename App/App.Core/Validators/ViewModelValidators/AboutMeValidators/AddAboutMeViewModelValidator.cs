using App.ViewModels.AdminMvc.AboutMeViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Core.Validators.ViewModelValidators.AboutMeValidators;
public class AddAboutMeViewModelValidator : AbstractValidator<AddAboutMeViewModel>
{
    public AddAboutMeViewModelValidator()
    {
        RuleFor(x => x.Introduction)
            .NotEmpty().WithMessage("Giriş kısmı boş olamaz.")
            .MaximumLength(100).WithMessage("Giriş maksimum 100 karakter olabilir.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Tam isim kısmı boş olamaz.")
            .MaximumLength(50).WithMessage("Tam isim maksimum 50 karakter olabilir.");

        RuleFor(x => x.Field)
            .NotEmpty().WithMessage("Alan kısmı boş olamaz.")
            .MaximumLength(50).WithMessage("Alan maksimum 50 karakter olabilir.");

        RuleFor(x => x.Image1)
            .NotNull().WithMessage("1. Fotoğraf kısmı boş olamaz.")
            .Must(BeAValidImage).WithMessage("1. Fotoğraf geçerli bir resim dosyası olmalıdır.");

        RuleFor(x => x.Image2)
            .NotNull().WithMessage("2. Fotoğraf kısmı boş olamaz.")
            .Must(BeAValidImage).WithMessage("2. Fotoğraf geçerli bir resim dosyası olmalıdır.");
    }

    private bool BeAValidImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return allowedExtensions.Contains(extension);
    }
}
