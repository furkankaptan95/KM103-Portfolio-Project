using App.ViewModels.PortfolioMvc.UserViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Core.Validators.ViewModelValidators.UserValidators;
public class EditUserImageViewModelValidator : AbstractValidator<EditUserImageViewModel>
{
    public EditUserImageViewModelValidator()
    {
        RuleFor(x => x.ImageFile)
           .NotNull().WithMessage("Fotoğraf kısmı boş olamaz.")
           .Must(BeAValidImage).WithMessage("Fotoğraf geçerli bir resim dosyası olmalıdır.");
    }

    private bool BeAValidImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return allowedExtensions.Contains(extension);
    }
}
