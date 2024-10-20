namespace App.DTOs.ContactMessageDtos.Admin;
public class ContactMessageToReplyDto
{
    public int MessageId { get; set; }
    public string Email { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; }
    public string Name { get; set; }
}
