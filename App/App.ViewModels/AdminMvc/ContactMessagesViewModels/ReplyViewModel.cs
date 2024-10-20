using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.ContactMessagesViewModels;
public class ReplyViewModel
{
    public GetContactMessageViewModel? GetModel { get; set; }
    [Required]
    public MessageReplyViewModel ReplyModel { get; set; }
}
