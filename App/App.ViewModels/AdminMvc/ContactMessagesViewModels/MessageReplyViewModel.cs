using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.ContactMessagesViewModels;
public class MessageReplyViewModel
{
    public int Id { get; set; }
    public string ReplyMessage { get; set; }
}
