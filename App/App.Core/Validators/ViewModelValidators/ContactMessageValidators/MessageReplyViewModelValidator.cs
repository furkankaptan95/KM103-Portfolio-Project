using App.ViewModels.AdminMvc.ContactMessagesViewModels;
using FluentValidation;

namespace App.Core.Validators.ViewModelValidators.ContactMessageValidators;
public class MessageReplyViewModelValidator : AbstractValidator<MessageReplyViewModel>
{
    public MessageReplyViewModelValidator()
    {
        RuleFor(x => x.ReplyMessage)
         .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Mesaj kısmı boş olamaz.");
    }
}
