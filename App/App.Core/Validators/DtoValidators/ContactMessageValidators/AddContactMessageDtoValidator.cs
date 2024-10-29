using App.DTOs.ContactMessageDtos.Portfolio;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.ContactMessageValidators;
public class AddContactMessageDtoValidator : AbstractValidator<AddContactMessageDto>
{
    public AddContactMessageDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("İsim kısmı boş olamaz.")
            .MaximumLength(50).WithMessage("İsim kısmı en fazla 50 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email kısmı zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email kısmı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Konu kısmı boş olamaz.")
            .MaximumLength(100).WithMessage("Konu kısmı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mesaj kısmı boş olamaz.");
    }
}
