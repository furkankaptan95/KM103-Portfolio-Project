using App.DTOs.ExperienceDtos.Admin;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.ExperienceValidators;
public class AddExperienceDtoValidator : AbstractValidator<AddExperienceDto>
{
    public AddExperienceDtoValidator()
    {
        RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Başlık kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Başlık maksimum 100 karakter olabilir.");

        RuleFor(x => x.Company)
           .NotEmpty().WithMessage("Şirket kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Şirket maksimum 100 karakter olabilir.");

        RuleFor(x => x.Description)
           .NotEmpty().WithMessage("Açıklama kısmı boş olamaz.");

        RuleFor(x => x.StartDate)
           .NotEmpty().WithMessage("Başlangıç tarihi gerekli.")  // Boş olamaz
           .Must(BeAValidStartDate).WithMessage("Geçerli bir tarih olmalı.");

        RuleFor(x => x.EndDate)
    .Must(BeAValidEndDate).When(x => x.EndDate.HasValue).WithMessage("Geçerli bir tarih olmalı."); // Değer girildiyse kontrol et

    }
    private bool BeAValidEndDate(DateTime? date)
    {
        return date.HasValue && date.Value != default;
    }

    private bool BeAValidStartDate(DateTime date)
    {
        return date != default;
    }

}
