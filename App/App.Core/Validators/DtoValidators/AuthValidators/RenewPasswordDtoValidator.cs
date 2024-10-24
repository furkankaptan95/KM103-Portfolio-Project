using App.DTOs.AuthDtos;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.AuthValidators;
public class RenewPasswordDtoValidator : AbstractValidator<RenewPasswordDto>
{
    public RenewPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email 100 karakterden uzun olamaz.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token alanı boş bırakılamaz.");
            
    }
}
