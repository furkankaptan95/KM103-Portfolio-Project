﻿using App.DTOs.EducationDtos;
using FluentValidation;

namespace App.Core.Validators.EducationValidators;
public class AddEducationDtoValidator : AbstractValidator<AddEducationDto>
{
    public AddEducationDtoValidator()
    {
        RuleFor(x => x.Degree)
           .NotEmpty().WithMessage("Derece kısmı boş olamaz.")
            .MaximumLength(100).WithMessage("Derece maksimum 50 karakter olabilir.");

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
        return date.HasValue && date.Value != default(DateTime);
    }

    private bool BeAValidStartDate(DateTime date)
    {
        return date != default(DateTime);
    }

}