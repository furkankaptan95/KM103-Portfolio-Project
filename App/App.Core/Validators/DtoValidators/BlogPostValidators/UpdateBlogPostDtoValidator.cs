using App.DTOs.BlogPostDtos.Admin;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.BlogPostValidators;

public class UpdateBlogPostDtoValidator : AbstractValidator<UpdateBlogPostDto>
{
    public UpdateBlogPostDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id bilgisi boş olamaz.")
             .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır.");

        RuleFor(x => x.Title)
           .NotEmpty().WithMessage("Başlık kısmı boş olamaz.")
            .MaximumLength(100).WithMessage("Başlık maksimum 100 karakter olabilir.");

        RuleFor(x => x.Content)
          .NotEmpty().WithMessage("İçerik kısmı boş olamaz.");
    }
}
