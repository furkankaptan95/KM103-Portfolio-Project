﻿using App.DTOs.PersonalInfoDtos;
using FluentValidation;

namespace App.Core.Validators.PersonalInfoValidators;
public class AddPersonalInfoDtoValidator : AbstractValidator<AddPersonalInfoDto>
{
    public AddPersonalInfoDtoValidator()
    {
        RuleFor(x => x.About)
         .NotEmpty().WithMessage("Hakkımda kısmı boş olamaz.")
          .MaximumLength(100).WithMessage("Hakkımda kısmı maksimum 300 karakter olabilir.");

        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("İsim kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("İsim maksimum 50 karakter olabilir.");

        RuleFor(x => x.Surname)
           .NotEmpty().WithMessage("Soyisim kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Soyisim maksimum 50 karakter olabilir.");

        RuleFor(x => x.BirthDate)
           .NotEmpty().WithMessage("Doğum tarihi gerekli.")
           .Must(BeAValidDate).WithMessage("Geçerli bir tarih giriniz.");
    }
    private bool BeAValidDate(DateTime date)
    {
        return date != default(DateTime);
    }
}