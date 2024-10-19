namespace App.DTOs.CommentDtos.Portfolio;
public class AddCommentUnsignedDto
{
    public string Content { get; set; }
    public int BlogPostId { get; set; }
    public string UnsignedCommenterName { get; set; }
}
