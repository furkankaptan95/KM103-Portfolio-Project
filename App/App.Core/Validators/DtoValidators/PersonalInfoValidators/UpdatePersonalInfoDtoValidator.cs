﻿using App.DTOs.PersonalInfoDtos;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.PersonalInfoValidators;
public class UpdatePersonalInfoDtoValidator : AbstractValidator<UpdatePersonalInfoDto>
{
    public UpdatePersonalInfoDtoValidator()
    {
        RuleFor(x => x.About)
         .NotEmpty().WithMessage("Hakkımda kısmı boş olamaz.")
          .MaximumLength(300).WithMessage("Hakkımda kısmı maksimum 300 karakter olabilir.");

        RuleFor(x => x.Name)
               .NotEmpty().WithMessage("İsim kısmı boş olamaz.")
               .MaximumLength(50).WithMessage("İsim maksimum 50 karakter olabilir.");

        RuleFor(x => x.Surname)
               .NotEmpty().WithMessage("Soyisim kısmı boş olamaz.")
               .MaximumLength(50).WithMessage("Soyisim maksimum 50 karakter olabilir.");

        RuleFor(x => x.BirthDate)
               .NotEmpty().WithMessage("Doğum tarihi gerekli.")
               .Must(BeAValidDate).WithMessage("Geçerli bir tarih giriniz.");
    }
    private bool BeAValidDate(DateTime date)
    {
        return date != default;
    }
}