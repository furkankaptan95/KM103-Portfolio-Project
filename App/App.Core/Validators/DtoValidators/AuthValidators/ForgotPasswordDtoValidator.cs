using App.DTOs.AuthDtos;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.AuthValidators;
public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email 100 karakterden uzun olamaz.");

        RuleFor(x => x.Url)
            .NotEmpty();
    }
}
