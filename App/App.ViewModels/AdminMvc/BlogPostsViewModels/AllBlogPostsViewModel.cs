namespace App.ViewModels.AdminMvc.BlogPostsViewModels;
public class AllBlogPostsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public bool IsVisible { get; set; }
}
