using FluentValidation;
using App.DTOs.AboutMeDtos;
using Microsoft.AspNetCore.Http;

namespace App.Core.Validators.AboutMeValidators;
public class AddAboutMeMVCDtoValidator : AbstractValidator<AddAboutMeMVCDto>
{
    public AddAboutMeMVCDtoValidator()
    {
        RuleFor(x=>x.Introduction)
            .NotEmpty().WithMessage("Giriş kısmı boş olamaz.")
             .Length(10, 1000).WithMessage("Başlık 10 ile 1000 karakter arasında olmalı.");

        RuleFor(x => x.ImageFile1)
            .NotNull().WithMessage("Resim boş olamaz.")
            .Must(BeAValidImage).WithMessage("Yalnızca .jpg, .jpeg, .png veya .gif uzantılı dosyalar yüklenebilir.");

        RuleFor(x => x.ImageFile2)
            .NotNull().WithMessage("Resim boş olamaz.")
            .Must(BeAValidImage).WithMessage("Yalnızca .jpg, .jpeg, .png veya .gif uzantılı dosyalar yüklenebilir.");
    }

    private bool BeAValidImage(IFormFile file)
    {
        // Geçerli uzantılar
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        // Dosya uzantısını kontrol et
        var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
        return validExtensions.Contains(extension);
    }
}
