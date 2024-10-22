using Microsoft.AspNetCore.Http;

namespace App.ViewModels.AdminMvc.ProjectsViewModels;
public class AddProjectViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile ImageFile { get; set; }

}
