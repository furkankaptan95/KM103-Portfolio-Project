using FluentValidation;
using App.AdminMVC.Models.AboutMeViewModels;
using Microsoft.AspNetCore.Http;

namespace App.Core.Validators.AboutMeViewModelsValidators;
public class AddAboutMeViewModelValidator : AbstractValidator<AddAboutMeViewModel>
{
    public AddAboutMeViewModelValidator()
    {
        RuleFor(x => x.Introduction)
            .NotEmpty().WithMessage("Introduction field is required.")
            .MinimumLength(10).WithMessage("Introduction must be at least 10 characters long.");

        RuleFor(x => x.Image1)
            .NotNull().WithMessage("Image 1 is required.")
            .Must(BeAValidImage).WithMessage("Image 1 must be a valid image file.");

        RuleFor(x => x.Image2)
            .NotNull().WithMessage("Image 2 is required.")
            .Must(BeAValidImage).WithMessage("Image 2 must be a valid image file.");
    }

    private bool BeAValidImage(IFormFile file)
    {
        if (file == null)
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        return allowedExtensions.Contains(extension);
    }
}
