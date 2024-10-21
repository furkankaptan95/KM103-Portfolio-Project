using App.DTOs.ProjectDtos.Admin;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.ProjectValidators;
public class AddProjectApiDtoValidator : AbstractValidator<AddProjectApiDto>
{
    public AddProjectApiDtoValidator()
    {
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl boş olamaz.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık kısmı boş olamaz.")
             .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama kısmı boş olamaz.");
    }
}
