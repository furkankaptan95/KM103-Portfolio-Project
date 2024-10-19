using App.ViewModels.PortfolioMvc.BlogPostsViewModels;

namespace App.ViewModels.PortfolioMvc;
public class HomeIndexViewModel
{
    public AboutMePortfolioViewModel? AboutMe { get; set; }
    public List<AllEducationsPortfolioViewModel>? Educations { get; set; }
    public List<AllExperiencesPortfolioViewModel>? Experiences { get; set; }
    public List<AllProjectsPortfolioViewModel>? Projects { get; set; }
    public PersonalInfoPortfolioViewModel? PersonalInfo { get; set; }
    public AddContactMessageViewModel? ContactMessage { get; set; }
    public List<HomeBlogPostsPortfolioViewModel>? BlogPosts { get; set; }

}
