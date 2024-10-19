using App.ViewModels.PortfolioMvc.CommentsViewModels;

namespace App.ViewModels.PortfolioMvc.BlogPostsViewModels;
public class BlogPostPagePortfolioViewModel
{
    public SingleBlogPostViewModel BlogPost { get; set; }
    public UnSignedAddCommentViewModel UnsignedComment { get; set; } = new UnSignedAddCommentViewModel();
    public SignedAddCommentViewModel SignedComment { get; set; } = new SignedAddCommentViewModel();
}
