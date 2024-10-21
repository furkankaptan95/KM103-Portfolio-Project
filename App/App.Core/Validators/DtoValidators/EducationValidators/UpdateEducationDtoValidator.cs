using App.DTOs.EducationDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Validators.DtoValidators.EducationValidators;
public class UpdateEducationDtoValidator : AbstractValidator<UpdateEducationDto>
{
    public UpdateEducationDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id bilgisi boş olamaz.")
             .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");

        RuleFor(x => x.Degree)
           .NotEmpty().WithMessage("Derece kısmı boş olamaz.")
            .MaximumLength(50).WithMessage("Derece maksimum 50 karakter olabilir.");

        RuleFor(x => x.School)
           .NotEmpty().WithMessage("Okul kısmı boş olamaz.")
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
