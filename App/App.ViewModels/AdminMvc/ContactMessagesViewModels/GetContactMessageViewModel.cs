using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.ContactMessagesViewModels;
public class GetContactMessageViewModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; }
    public string Name { get; set; }

}
