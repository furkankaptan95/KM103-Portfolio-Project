using App.Core.Entities;
namespace App.Data.Entities;
public class BlogPostEntity : BaseEntity<int>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsVisible { get; set; } = true;
    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}
