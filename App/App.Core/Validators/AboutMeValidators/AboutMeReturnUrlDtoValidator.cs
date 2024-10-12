using App.DTOs.FileApiDtos;
using FluentValidation;

namespace App.Core.Validators.AboutMeValidators;
public class AboutMeReturnUrlDtoValidator : AbstractValidator<ReturnUrlDto>
{
    public AboutMeReturnUrlDtoValidator()
    {
        RuleFor(x => x.ImageUrl1).NotEmpty().WithMessage("Image1Url boş olamaz.");

        RuleFor(x => x.ImageUrl2).NotEmpty().WithMessage("Image1Url boş olamaz.");
    }
}
