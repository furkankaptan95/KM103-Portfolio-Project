using App.Core.Entities;
namespace App.Data.Entities;
public class ContactMessageEntity : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; }
    public bool IsRead { get; set; } = false;
    public string? Reply { get; set; }
    public DateTime? ReplyDate { get; set; }

}
