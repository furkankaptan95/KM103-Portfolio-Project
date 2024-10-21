using App.ViewModels.AdminMvc.ExperiencesViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.ExperienceValidators;

public class UpdateExperienceViewModelValidator : AbstractValidator<UpdateExperienceViewModel>
{
    public UpdateExperienceViewModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id bilgisi boş olamaz.")
             .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");

        RuleFor(x => x.Title)
         .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Başlık kısmı boş olamaz.")
          .MaximumLength(100).WithMessage("Başlık maksimum 100 karakter olabilir.");

        RuleFor(x => x.Company)
           .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Şirket kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Şirket maksimum 100 karakter olabilir.");

        RuleFor(x => x.Description)
           .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Açıklama kısmı boş olamaz.");

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
