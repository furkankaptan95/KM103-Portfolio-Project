using App.DTOs.CommentDtos.Portfolio;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.CommentValidators;
public class AddCommentUnsignedDtoValidator : AbstractValidator<AddCommentUnsignedDto>
{
    public AddCommentUnsignedDtoValidator()
    {
        RuleFor(comment => comment.Content)
            .NotEmpty().WithMessage("İçerik kısmı boş olamaz.")
             .MaximumLength(300).WithMessage("İçerik maksimum 300 karakter olabilir.");

        RuleFor(comment => comment.UnsignedCommenterName)
            .NotEmpty().WithMessage("İsim kısmı boş olamaz.")
             .MaximumLength(50).WithMessage("İsim maksimum 50 karakter olabilir.");

        RuleFor(comment => comment.BlogPostId)
            .GreaterThan(0).WithMessage("0'dan büyük bir BlogPostId gerekli.");
    }
}
