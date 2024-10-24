using App.ViewModels.AuthViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.AuthValidators;
public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        // Email validasyonu
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email 100 karakterden uzun olamaz.");

        // Şifre validasyonu
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .MaximumLength(15).WithMessage("Şifre en fazla 15 karakter olabilir.");
    }
}
