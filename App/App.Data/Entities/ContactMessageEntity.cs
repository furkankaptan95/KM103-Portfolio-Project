using App.Core.Entities;
namespace App.Data.Entities;
public class ContactMessageEntity : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SentDate { get; set; }
    public bool IsRead { get; set; } = false;
    public string? Reply { get; set; }
    public DateTime? ReplyDate { get; set; }

}
