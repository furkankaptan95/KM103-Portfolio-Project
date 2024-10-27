using App.DTOs.AuthDtos;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.AuthValidators;
public class VerifyEmailDtoValidator : AbstractValidator<VerifyEmailDto>
{
    public VerifyEmailDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email 100 karakterden uzun olamaz.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token alanı boş olamaz.")
            .Length(6).WithMessage("Token tam olarak 6 karakter olmalıdır.");
    }
}
