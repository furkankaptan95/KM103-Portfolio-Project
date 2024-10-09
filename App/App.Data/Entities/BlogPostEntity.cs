using App.Core.Entities;
namespace App.Data.Entities;
public class BlogPostEntity : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public bool IsVisible { get; set; } = true;
    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}
