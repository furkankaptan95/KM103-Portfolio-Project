using App.DTOs.AuthDtos;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.AuthValidators;
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
          .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
          .MaximumLength(100).WithMessage("Email 100 karakterden uzun olamaz.");

        RuleFor(x => x.Username)
          .NotEmpty().WithMessage("Kullanıcı adı alanı boş bırakılamaz.")
          .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .MaximumLength(15).WithMessage("Şifre en fazla 15 karakter olabilir.");
    }
}
