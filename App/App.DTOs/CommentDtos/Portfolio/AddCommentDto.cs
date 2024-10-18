namespace App.DTOs.CommentDtos.Portfolio;
public class AddCommentDto
{
    public string Content { get; set; }
    public int? UserId { get; set; }
    public int BlogPostId { get; set; }
    public string? UnsignedCommenterName { get; set; }
}
