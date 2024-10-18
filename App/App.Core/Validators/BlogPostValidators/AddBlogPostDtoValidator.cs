using App.DTOs.BlogPostDtos.Admin;
using FluentValidation;

namespace App.Core.Validators.BlogPostValidators;

public class AddBlogPostDtoValidator : AbstractValidator<AddBlogPostDto>
{
    public AddBlogPostDtoValidator()
    {
        RuleFor(x => x.Title)
           .NotEmpty().WithMessage("Başlık kısmı boş olamaz.")
            .MaximumLength(100).WithMessage("Başlık maksimum 100 karakter olabilir.");

        RuleFor(x => x.Content)
           .NotEmpty().WithMessage("İçerik kısmı boş olamaz.");
    }
}
