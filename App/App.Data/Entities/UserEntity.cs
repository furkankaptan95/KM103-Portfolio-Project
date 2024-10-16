using App.Core.Entities;
namespace App.Data.Entities;
public class UserEntity : BaseEntity<int>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public string? ImageUrl { get; set; } = "user-img.jpg";
    public byte[]? PasswordHash { get; set; } = default!;
    public byte[]? PasswordSalt { get; set; } = default!;
    public string Role { get; set; } = "commenter";
   
}

