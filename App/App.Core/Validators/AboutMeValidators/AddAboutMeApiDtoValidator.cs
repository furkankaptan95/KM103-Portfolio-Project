﻿using App.DTOs.AboutMeDtos;
using FluentValidation;

namespace App.Core.Validators.AboutMeValidators;
public class AddAboutMeApiDtoValidator : AbstractValidator<AddAboutMeApiDto>
{
    public AddAboutMeApiDtoValidator()
    {
        RuleFor(x => x.ImageUrl1).NotEmpty().WithMessage("Image1Url boş olamaz.");

        RuleFor(x => x.ImageUrl2).NotEmpty().WithMessage("Image2Url boş olamaz.");

        RuleFor(x => x.Introduction)
            .NotEmpty().WithMessage("Giriş kısmı boş olamaz.")
             .Length(10, 1000).WithMessage("Başlık 10 ile 1000 karakter arasında olmalı.");
    }
}