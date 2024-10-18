namespace App.DTOs.CommentDtos.Admin;
public class AllCommentsAdminDto
{
    public int Id { get; set; }
    public string Commenter { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsApproved { get; set; }
    public string BlogPostName { get; set; }
}
