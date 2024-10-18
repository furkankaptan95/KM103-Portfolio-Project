namespace App.ViewModels.AdminMvc.BlogPostsViewModels;
public class AdminAllBlogPostsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public bool IsVisible { get; set; }
}
