using App.ViewModels.PortfolioMvc.CommentsViewModels;

namespace App.ViewModels.PortfolioMvc.BlogPostsViewModels;
public class BlogPostPagePortfolioViewModel
{
    public SingleBlogPostViewModel? BlogPost { get; set; }
    public UnSignedAddCommentViewModel UnsignedComment { get; set; }
    public SignedAddCommentViewModel SignedComment { get; set; }
}
