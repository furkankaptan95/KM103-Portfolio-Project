using App.DTOs.ContactMessageDtos.Admin;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.ContactMessageValidators;
public class ReplyContactMessageDtoValidator : AbstractValidator<ReplyContactMessageDto>
{
    public ReplyContactMessageDtoValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty().WithMessage("Id bilgisi boş olamaz.")
            .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");

        RuleFor(x => x.ReplyMessage)
           .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Mesaj kısmı boş olamaz.");
    }
}
