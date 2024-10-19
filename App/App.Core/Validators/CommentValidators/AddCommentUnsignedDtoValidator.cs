using App.DTOs.CommentDtos.Portfolio;
using FluentValidation;

namespace App.Core.Validators.CommentValidators;
public class AddCommentUnsignedDtoValidator : AbstractValidator<AddCommentUnsignedDto>
{
    public AddCommentUnsignedDtoValidator()
    {
        RuleFor(comment => comment.Content)
            .NotEmpty().WithMessage("İçerik kısmı zorunludur.")
            .Must(content => !string.IsNullOrWhiteSpace(content)).WithMessage("İçerik boş olamaz.");

        RuleFor(comment => comment.UnsignedCommenterName)
            .NotEmpty().WithMessage("İsim zorunludur.")
            .Must(content => !string.IsNullOrWhiteSpace(content)).WithMessage("İsim boş olamaz.");

        RuleFor(comment => comment.BlogPostId)
            .GreaterThan(0).WithMessage("0'dan büyük bir BlogPostId gerekli.");
    }
}
