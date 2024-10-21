using App.ViewModels.AdminMvc.EducationsViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.EducationValidators;
public class AddEducationViewModelValidator : AbstractValidator<AddEducationViewModel>
{
    public AddEducationViewModelValidator()
    {
        RuleFor(x => x.Degree)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Derece kısmı boş olamaz.")
            .MaximumLength(50).WithMessage("Derece maksimum 50 karakter olabilir.");

        RuleFor(x => x.School)
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Okul kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Okul maksimum 100 karakter olabilir.");

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
