using App.DTOs.AboutMeDtos;
using FluentValidation;

namespace App.Core.Validators.AboutMeValidators;
public class UpdateAboutMeApiDtoValidator : AbstractValidator<UpdateAboutMeApiDto>
{
    public UpdateAboutMeApiDtoValidator()
    {
        RuleFor(x => x.Introduction)
           .NotEmpty().WithMessage("Giriş kısmı boş olamaz.")
            .Length(10, 1000).WithMessage("Giriş 10 ile 1000 karakter arasında olmalı.");
    }
}
