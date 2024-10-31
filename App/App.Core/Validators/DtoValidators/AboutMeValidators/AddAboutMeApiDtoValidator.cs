using App.DTOs.AboutMeDtos.Admin;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.AboutMeValidators;
public class AddAboutMeApiDtoValidator : AbstractValidator<AddAboutMeApiDto>
{
    public AddAboutMeApiDtoValidator()
    {
        RuleFor(x => x.ImageUrl1)
             .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("ImageUrl1 kısmı boş olamaz.");

        RuleFor(x => x.ImageUrl2)
             .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("ImageUrl2 kısmı boş olamaz.");

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
}
