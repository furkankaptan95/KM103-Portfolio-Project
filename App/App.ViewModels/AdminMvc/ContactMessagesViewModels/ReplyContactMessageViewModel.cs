namespace App.ViewModels.AdminMvc.ContactMessagesViewModels;
public class ReplyContactMessageViewModel
{
    public int MessageId { get; set; }
    public string Email { get; set; }
    public string? Subject { get; set; }
    public string ReplyMessage { get; set; }
    public string? Message { get; set; }
    public DateTime? SentDate { get; set; }
    public string? Name { get; set; }

}
