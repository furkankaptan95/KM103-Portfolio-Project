using App.DTOs.UserDtos;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.UserValidators;
public class EditUsernameDtoValidator : AbstractValidator<EditUsernameDto>
{
    public EditUsernameDtoValidator()
    {
        RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
          .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
          .MaximumLength(100).WithMessage("Email 100 karakterden uzun olamaz.");

        RuleFor(x => x.Username)
       .NotEmpty().WithMessage("Kullanıcı adı alanı boş bırakılamaz.")
       .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");
    }
}
