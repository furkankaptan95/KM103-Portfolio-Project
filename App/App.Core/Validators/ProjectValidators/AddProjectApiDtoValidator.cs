using App.DTOs.ProjectDtos;
using FluentValidation;

namespace App.Core.Validators.ProjectValidators;
public class AddProjectApiDtoValidator : AbstractValidator<AddProjectApiDto>
{
    public AddProjectApiDtoValidator()
    {
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl boş olamaz.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Giriş kısmı boş olamaz.")
             .Length(10, 1000).WithMessage("Başlık 10 ile 1000 karakter arasında olmalı.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama kısmı boş olamaz.");
    }
}
