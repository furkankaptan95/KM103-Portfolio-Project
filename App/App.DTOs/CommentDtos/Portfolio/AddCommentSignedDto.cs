namespace App.DTOs.CommentDtos.Portfolio;
public class AddCommentSignedDto
{
    public string Content { get; set; }
    public int UserId { get; set; }
    public int BlogPostId { get; set; }
}
