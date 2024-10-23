using App.Core.Entities;

namespace App.Data.Entities;
public class UserVerificationEntity : BaseEntity<int>
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual UserEntity User { get; set; }
}
