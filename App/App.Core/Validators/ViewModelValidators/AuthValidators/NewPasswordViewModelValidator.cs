using App.ViewModels.AuthViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.AuthValidators;
public class NewPasswordViewModelValidator : AbstractValidator<NewPasswordViewModel>
{
    public NewPasswordViewModelValidator()
    {
        RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email alanı zorunludur.")
      .EmailAddress().WithMessage("Geçerli bir email adresi girin.")
      .MaximumLength(100).WithMessage("Email alanı en fazla 100 karakter olmalıdır.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı zorunludur.")
            .Length(8, 15).WithMessage("Şifre 8 ile 15 karakter arasında olmalıdır.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Şifre tekrar alanı zorunludur.")
            .Equal(x => x.Password).WithMessage("Şifreler eşleşmelidir.");
    }
}
