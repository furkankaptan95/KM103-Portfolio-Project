using App.DTOs.AboutMeDtos;
using FluentValidation;

namespace App.Core.Validators.AboutMeValidators;
public class UpdateAboutMeApiDtoValidator : AbstractValidator<UpdateAboutMeApiDto>
{
    public UpdateAboutMeApiDtoValidator()
    {
        RuleFor(x => x.ImageUrl1)
           .Must(name => name == null || !string.IsNullOrWhiteSpace(name))
           .WithMessage("Image1Url kısmı boş olamaz.");

        RuleFor(x => x.ImageUrl2)
            .Must(name => name == null || !string.IsNullOrWhiteSpace(name))
          .WithMessage("Image2Url kısmı boş olamaz.");

        RuleFor(x => x.Introduction)
             .MaximumLength(100).WithMessage("Giriş maksimum 100 karakter olabilir.")
             .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Giriş kısmı boş olamaz.");

        RuleFor(x => x.FullName)
            .MaximumLength(100).WithMessage("Tam isim maksimum 50 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Tam isim kısmı boş olamaz.");

        RuleFor(x => x.Field)
            .MaximumLength(50).WithMessage("Alan maksimum 50 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Alan kısmı boş olamaz.");
    }
}
