using App.DTOs.ContactMessageDtos.Portfolio;
using FluentValidation;

namespace App.Core.Validators.ContactMessageValidators;
public class AddContactMessageDtoValidator : AbstractValidator<AddContactMessageDto>
{
    public AddContactMessageDtoValidator()
    {
        
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("İsim kısmı en fazla 50 karakter olabilir.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("İsim kısmı boş olamaz.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email kısmı zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email kısmı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Subject)
            .MaximumLength(100).WithMessage("Konu kısmı en fazla 100 karakter olabilir.")
            .Must(name => name == null || !string.IsNullOrWhiteSpace(name)).WithMessage("Konu kısmı boşluk olamaz.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mesaj kısmı zorunludur.")
            .Must(message => !string.IsNullOrWhiteSpace(message)).WithMessage("Mesaj kısmı boş olamaz.");
    }
}
