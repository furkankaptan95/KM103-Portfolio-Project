using App.ViewModels.PortfolioMvc.UserViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.UserValidators;
public class EditUsernameViewModelValidator : AbstractValidator<EditUsernameViewModel>
{
    public EditUsernameViewModelValidator()
    {
        RuleFor(x => x.Username)
         .NotEmpty().WithMessage("Kullanıcı adı alanı boş bırakılamaz.")
         .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");
    }
}
