using App.Core.Entities;
namespace App.Data.Entities;
public class CommentEntity : BaseEntity<int>
{
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsApproved { get; set; } = false;
    public int? BlogPostId { get; set; }
    public BlogPostEntity? BlogPost { get; set; }
    public int? UserId { get; set; }
    public string? UnsignedCommenterName { get; set; }

}
