using App.DTOs.CommentDtos.Portfolio;

namespace App.DTOs.BlogPostDtos.Porfolio;
public class SingleBlogPostDto
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Content { get; set; }
	public DateTime PublishDate { get; set; }
	public List<BlogPostCommentsPortfolioDto>? Comments { get; set; }
}
