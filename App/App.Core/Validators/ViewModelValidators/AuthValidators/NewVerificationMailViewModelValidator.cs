using App.ViewModels.AuthViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.AuthValidators;
public class NewVerificationMailViewModelValidator : AbstractValidator<NewVerificationMailViewModel>
{
    public NewVerificationMailViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email 100 karakterden uzun olamaz.");
    }
}
