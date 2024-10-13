using App.DTOs.BlogPostDtos;
using FluentValidation;

namespace App.Core.Validators.BlogPostValidators;

public class UpdateBlogPostDtoValidator : AbstractValidator<UpdateBlogPostDto>
{
    public UpdateBlogPostDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id bilgisi boş olamaz.");

        RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Başlık kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Başlık maksimum 100 karakter olabilir.");

        RuleFor(x => x.Content)
           .NotEmpty().WithMessage("İçerik kısmı boş olamaz.");
    }
}
