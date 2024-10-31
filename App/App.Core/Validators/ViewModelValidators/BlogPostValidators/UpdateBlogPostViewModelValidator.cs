using App.ViewModels.AdminMvc.BlogPostsViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.BlogPostValidators;
public class UpdateBlogPostViewModelValidator : AbstractValidator<UpdateBlogPostViewModel>
{
	public UpdateBlogPostViewModelValidator()
	{

        RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Başlık kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Başlık maksimum 100 karakter olabilir.");

        RuleFor(x => x.Content)
          .NotEmpty().WithMessage("İçerik kısmı boş olamaz.");
    }
}
