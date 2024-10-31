using App.ViewModels.AdminMvc.BlogPostsViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.BlogPostValidators;
public class AddBlogPostViewModelValidator : AbstractValidator<AddBlogPostViewModel>
{
    public AddBlogPostViewModelValidator()
    {
        RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Başlık kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Başlık maksimum 100 karakter olabilir.");

        RuleFor(x => x.Content)
          .NotEmpty().WithMessage("İçerik kısmı boş olamaz.");
    }
}
