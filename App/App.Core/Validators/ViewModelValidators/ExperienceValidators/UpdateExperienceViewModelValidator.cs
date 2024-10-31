using App.ViewModels.AdminMvc.ExperiencesViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.ExperienceValidators;

public class UpdateExperienceViewModelValidator : AbstractValidator<UpdateExperienceViewModel>
{
    public UpdateExperienceViewModelValidator()
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
               .Must(BeAValidEndDate).When(x => x.EndDate.HasValue).WithMessage("Geçerli bir tarih olmalı.") // Değer girildiyse kontrol et
               .Must((viewModel, endDate) => !endDate.HasValue || endDate >= viewModel.StartDate)
               .WithMessage("Bitiş tarihi, başlangıç tarihinden önce olamaz.");
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
