using App.DTOs.AboutMeDtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Core.Validators.AboutMeValidators;
public class UpdateAboutMeMVCDtoValidator : AbstractValidator<UpdateAboutMeMVCDto>
{
    public UpdateAboutMeMVCDtoValidator()
    {
        RuleFor(x => x.Introduction)
            .NotEmpty().WithMessage("Giriş kısmı boş olamaz.")
            .Length(10, 1000).WithMessage("Başlık 10 ile 1000 karakter arasında olmalı.");

        RuleFor(x => x.ImageFile1)
            .Must(BeAValidImage).When(x => x.ImageFile1 != null)
            .WithMessage("Yalnızca .jpg, .jpeg, .png veya .gif uzantılı dosyalar yüklenebilir.");


        RuleFor(x => x.ImageFile2)
             .Must(BeAValidImage).When(x => x.ImageFile2 != null)
             .WithMessage("Yalnızca .jpg, .jpeg, .png veya .gif uzantılı dosyalar yüklenebilir.");
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
