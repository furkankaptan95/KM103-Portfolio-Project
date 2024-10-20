using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.ContactMessagesViewModels;
public class MessageReplyViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Yanıt kısmı zorunludur.")]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "Yanıt sadece boşluk olamaz.")]
    public string ReplyMessage { get; set; }
}
