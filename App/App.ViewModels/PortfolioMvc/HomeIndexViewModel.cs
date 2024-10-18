using App.ViewModels.PortfolioMvc.BlogPostsViewModels;

namespace App.ViewModels.PortfolioMvc;
public class HomeIndexViewModel
{
    public AboutMePortfolioViewModel? AboutMe { get; set; }
    public AllEducationsPortfolioViewModel? Educations { get; set; }
    public AllExperiencesPortfolioViewModel? Experiences { get; set; }
    public AllProjectsPortfolioViewModel? Projects { get; set; }
    public PersonalInfoPortfolioViewModel? PersonalInfo { get; set; }
    public AddContactMessageViewModel? ContactMessage { get; set; }
    public AllBlogPostsPortfolioViewModel? BlogPosts { get; set; }

}
