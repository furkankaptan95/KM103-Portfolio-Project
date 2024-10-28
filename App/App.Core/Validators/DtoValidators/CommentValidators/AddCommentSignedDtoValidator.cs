using App.DTOs.CommentDtos.Portfolio;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.CommentValidators;
public class AddCommentSignedDtoValidator : AbstractValidator<AddCommentSignedDto>
{
    public AddCommentSignedDtoValidator()
    {
        RuleFor(comment => comment.Content)
            .NotEmpty().WithMessage("İçerik kısmı boş olamaz.")
            .MaximumLength(300).WithMessage("İçerik maksimum 300 karakter olabilir.");

        RuleFor(comment => comment.UserId)
            .GreaterThan(0).WithMessage("0'dan büyük bir UserId gerekli.");

        RuleFor(comment => comment.BlogPostId)
            .GreaterThan(0).WithMessage("0'dan büyük bir BlogPostId gerekli.");
    }
}
