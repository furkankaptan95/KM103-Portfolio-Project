using App.DTOs.CommentDtos.Portfolio;
using FluentValidation;

namespace App.Core.Validators.CommentValidators;
public class AddCommentSignedDtoValidator : AbstractValidator<AddCommentSignedDto>
{
	public AddCommentSignedDtoValidator()
	{
        RuleFor(comment => comment.Content)
            .NotEmpty().WithMessage("İçerik kısmı zorunludur.")
            .Must(content => !string.IsNullOrWhiteSpace(content)).WithMessage("İçerik boş olamaz.");

        RuleFor(comment => comment.UserId)
            .GreaterThan(0).WithMessage("0'dan büyük bir UserId gerekli.");

        RuleFor(comment => comment.BlogPostId)
            .GreaterThan(0).WithMessage("0'dan büyük bir BlogPostId gerekli.");
    }
}
