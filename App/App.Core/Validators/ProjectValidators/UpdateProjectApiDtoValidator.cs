using App.DTOs.ProjectDtos;
using FluentValidation;

namespace App.Core.Validators.ProjectValidators;
public class UpdateProjectApiDtoValidator : AbstractValidator<UpdateProjectApiDto>
{
    public UpdateProjectApiDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id bilgisi boş olamaz.")
             .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık kısmı boş olamaz.")
             .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama kısmı boş olamaz.");
    }
}
