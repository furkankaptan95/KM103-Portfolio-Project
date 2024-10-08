using App.Core.Entities;
namespace App.Data.Entities;
public class CommentEntity : BaseEntity<int>
{
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsApproved { get; set; } = false;
    public int BlogPostId { get; set; }
    public BlogPostEntity BlogPost { get; set; }
    public int? UserId { get; set; }
    public UserEntity? User { get; set; }
    public string? UnsignedCommenterName { get; set; }

}
