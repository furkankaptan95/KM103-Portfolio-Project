using App.DTOs.UserDtos;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.UserValidators;
public class EditUserImageApiDtoValidator : AbstractValidator<EditUserImageApiDto>
{
    public EditUserImageApiDtoValidator()
    {
        RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
          .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
          .MaximumLength(100).WithMessage("Email 100 karakterden uzun olamaz.");

        RuleFor(x => x.ImageUrl)
           .NotEmpty().WithMessage("ImageUrl kısmı boş olamaz.")
            .MaximumLength(255).WithMessage("ImageUrl 255 karakterden uzun olamaz.");
    }
}
