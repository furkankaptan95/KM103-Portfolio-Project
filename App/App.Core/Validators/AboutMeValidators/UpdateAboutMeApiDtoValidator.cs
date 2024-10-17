using App.DTOs.AboutMeDtos;
using FluentValidation;

namespace App.Core.Validators.AboutMeValidators;
public class UpdateAboutMeApiDtoValidator : AbstractValidator<UpdateAboutMeApiDto>
{
    public UpdateAboutMeApiDtoValidator()
    {
        RuleFor(x => x.Introduction)
           .NotEmpty().WithMessage("Giriş kısmı boş olamaz.");
    }
}
