using Microsoft.AspNetCore.Http;

namespace App.ViewModels.AdminMvc.ProjectsViewModels;
public class UpdateProjectViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string ImageUrl { get; set; }
}
