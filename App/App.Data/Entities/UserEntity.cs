using App.Core.Entities;
namespace App.Data.Entities;
public class UserEntity : BaseEntity<int>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = false;
    public string? ImageUrl { get; set; }
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
    public string Role { get; set; } = "commenter";
    public virtual ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = default!;

}

