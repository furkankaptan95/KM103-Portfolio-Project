using App.DTOs.ProjectDtos.Admin;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.ProjectValidators;
public class AddProjectApiDtoValidator : AbstractValidator<AddProjectApiDto>
{
    public AddProjectApiDtoValidator()
    {
        RuleFor(x => x.ImageUrl)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("ImageUrl kısmı boş olamaz.");

        RuleFor(x => x.Title)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Başlık kısmı boş olamaz.")
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Açıklama kısmı boş olamaz.");
    }
}
