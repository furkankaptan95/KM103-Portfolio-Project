namespace App.DTOs.CommentDtos.Portfolio;
public class BlogPostCommentsPortfolioDto
{
    public int Id { get; set; }
    public string Commenter { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
