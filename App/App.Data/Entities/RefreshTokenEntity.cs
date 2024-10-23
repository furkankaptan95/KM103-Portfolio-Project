using App.Core.Entities;

namespace App.Data.Entities;
public class RefreshTokenEntity : BaseEntity<int>
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public DateTime? IsUsed { get; set; }
    public DateTime? IsRevoked { get; set; }
    public int UserId { get; set; }
    public virtual UserEntity User { get; set; }
}